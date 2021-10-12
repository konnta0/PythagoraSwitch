using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using konnta0.Exceptions;
using Microsoft.Extensions.Logging;
using Polly;
using PythagoraSwitch.Recorder;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public sealed class WebRequestHandler : IWebRequestHandler, IPsWebRequesting
    {
        private readonly ILogger<WebRequestHandler> _logger;
        private readonly IPsNetworkAccess _networkAccess;
        private readonly IPsConfig _config;
        private readonly IPsSerializer _serializer;
        private readonly IPsRequestQueue _requestQueue;
        private readonly IPsHttpClientFactory _httpClientFactory;
        private readonly IPsRecorder _recorder;

        public bool Doing { get; private set; }
        
        public WebRequestHandler(
            ILogger<WebRequestHandler> logger,
            IPsNetworkAccess networkAccess,
            IPsConfig config,
            IPsSerializer serializer,
            IPsRequestQueue requestQueue,
            IPsHttpClientFactory httpClientFactory,
            IPsRecorder recorder)
        {
            _logger = logger;
            _networkAccess = networkAccess;
            _config = config;
            _serializer = serializer;
            _requestQueue = requestQueue;
            _httpClientFactory = httpClientFactory;
            _recorder = recorder;
            if (_config.RequestRecording)
            {
                _ = _recorder?.Start();
            }

            var tokenSource = new CancellationTokenSource();
            _requestQueue.WatchRequestQueue(_config.QueueWatchDelayMilliseconds, HandleRequest, tokenSource.Token);
        }

        private async void HandleRequest(IPsRequest request)
        {
            OnChangeRequesting?.Invoke(Doing = true);
            OnStartRequest?.Invoke(request);
            var (responseMessage, error) = await request.HandleTask;
            request.OnResponse((responseMessage, error));
            OnChangeRequesting?.Invoke(Doing = false);
        }
        
        public async Task<(TRes, IErrors)> PostAsync<TReq, TRes>(Uri uri, TReq body, IPsWebRequestConfig overwriteConfig = null) 
            where TReq : IPsWebPostRequestContent where TRes : IPsWebResponseContent
        {
            var validNetworkAccess = ValidNetworkAccess();
            if (validNetworkAccess != null)
            {
                return (default, validNetworkAccess);
            }
            var isDone = false;
            TRes httpResponse = default;
            IErrors error = null;
            
            var request = new Request
            {
                HandleTask = RequestPostTask(uri, body, overwriteConfig),
                OnResponse = tuple =>
                {
                    var (responseMessage, requestError) = tuple;
                    if (Errors.IsOccurred(requestError))
                    {
                        error = requestError;
                    }
                    else
                    {
                       (httpResponse, error) = _serializer.Deserialize<TRes>(responseMessage);
                    }

                    isDone = true;
                }
            };

            _requestQueue.Enqueue(request);
            while (!isDone)
            {
                await Task.Delay(50);
            }
            return (httpResponse, error);
        }

        private async Task<(string, IErrors)> RequestPostTask<TReq>(Uri uri, TReq body, IPsWebRequestConfig overwriteConfig = null)
            where TReq : IPsWebPostRequestContent
        {
            var validNetworkAccess = ValidNetworkAccess();
            if (validNetworkAccess != null)
            {
                return (default, validNetworkAccess);
            }

            var requestConfig = overwriteConfig ?? _config; 

            if (_config.RequestRecording)
            {
                var urlBuilder = new UriBuilder(uri);
                var requestRecordContent = new PsRequestRecordContent
                {
                    Method = HttpMethod.Post.ToString(),
                    EndPoint = urlBuilder.Path,
                    RequestContent = body,
                    RequestContentType = typeof(TReq),
                    Headers = requestConfig.Headers
                };
                requestRecordContent.RequestStart();
                _recorder.Add(requestRecordContent);
            }

            var message = string.Empty;
            async Task<IErrors> RequestTask()
            {
                var (str, serializedError) = _serializer.Serialize(body);
                if (Errors.IsOccurred(serializedError))
                {
                    return serializedError;
                }
                _logger.LogInformation("[Http] REQUEST method:POST url:{Url}", uri.ToString());
                var client = CreateClient(requestConfig);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = new StringContent(str, Encoding.UTF8, _serializer.ContentType)
                };
                foreach (var (headerKey, headerValues) in requestConfig.Headers)
                {
                    requestMessage.Headers.Add(headerKey, headerValues);
                }

                using var responseMessage = await Policy
                    .HandleResult<HttpResponseMessage>(x => requestConfig.RetryHttpStatusCodes.Contains(x.StatusCode))
                    .WaitAndRetryAsync(requestConfig.RetryCount, requestConfig.RetrySleepDurationProvider)
                    .ExecuteAsync(() => client.SendAsync(requestMessage));
                _logger.LogInformation("[Http] RESPONSE method:POST url:{Url} statusCode:{Status}", uri.ToString(), responseMessage.StatusCode);
                if (!responseMessage.IsSuccessStatusCode)
                {
                    return Errors.New(new Exception($"request failed status code {responseMessage.StatusCode}"));
                }

                message = await responseMessage.Content.ReadAsStringAsync();
                return Errors.Nothing();
            }
            
            var error = await Errors.TryTask(RequestTask());

            if (Errors.IsOccurred(error))
            {
                return (string.Empty, error);
            }

            return (message, Errors.Nothing());
        }

        public async Task<(TRes, IErrors)> GetAsync<TGetReq, TRes>(Uri uri, TGetReq queryObject, IPsWebRequestConfig overwriteConfig = null)
            where TGetReq : IPsWebGetRequestContent where TRes : IPsWebResponseContent
        {
            var validNetworkAccessError = ValidNetworkAccess();
            if (validNetworkAccessError != null)
            {
                return (default, validNetworkAccessError);
            }
            
            var isDone = false;
            TRes httpResponse = default;
            IErrors error = null;
            
            var request = new Request
            {
                HandleTask = RequestGetTask(uri, queryObject, overwriteConfig),
                OnResponse = tuple =>
                {
                    var (responseMessage, requestError) = tuple;
                    if (Errors.IsOccurred(requestError))
                    {
                        error = requestError;
                    }
                    else
                    {
                        (httpResponse, error) = _serializer.Deserialize<TRes>(responseMessage);
                    }
                    isDone = true;  
                }
            };

            _requestQueue.Enqueue(request);
            while (!isDone)
            {
                await Task.Delay(50);
            }

            return (httpResponse, error);
        }

        private async Task<(string, IErrors)> RequestGetTask<TGetReq>(Uri uri, TGetReq queryObject, IPsWebRequestConfig overwriteConfig = null)
            where TGetReq : IPsWebGetRequestContent
        {
            var validNetworkAccess = ValidNetworkAccess();
            if (validNetworkAccess != null)
            {
                return (default, validNetworkAccess);
            }

            var requestConfig = overwriteConfig ?? _config; 

            if (_config.RequestRecording)
            {
                var urlBuilder = new UriBuilder(uri);
                var requestRecordContent = new PsRequestRecordContent
                {
                    Method = HttpMethod.Get.ToString(),
                    EndPoint = urlBuilder.Path,
                    RequestContent = queryObject,
                    RequestContentType = typeof(TGetReq),
                    Headers = requestConfig.Headers
                };
                requestRecordContent.RequestStart();
                _recorder.Add(requestRecordContent);
            }

            var requestUrl = $"{uri}&{queryObject.ToQueryString()}";

            var message = string.Empty;
            async Task<IErrors> RequestTask()
            {
                var client = CreateClient(requestConfig);
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                foreach (var (key, value) in requestConfig.Headers)
                {
                    requestMessage.Headers.Add(key, value);
                }

                _logger.LogInformation("[Http] REQUEST method:GET url:{Url}", requestUrl);

                using var responseMessage = await Policy
                    .HandleResult<HttpResponseMessage>(x => requestConfig.RetryHttpStatusCodes.Contains(x.StatusCode))
                    .WaitAndRetryAsync(requestConfig.RetryCount, requestConfig.RetrySleepDurationProvider)
                    .ExecuteAsync(() => client.SendAsync(requestMessage));
                _logger.LogInformation(
                    "[Http] RESPONSE method:GET url:{Url} statusCode:{Status}", requestUrl, responseMessage.StatusCode);
                if (!responseMessage.IsSuccessStatusCode)
                {
                    return Errors.New(new Exception($"request failed status code {responseMessage.StatusCode}"));
                }

                message = await responseMessage.Content.ReadAsStringAsync();
                return Errors.Nothing();
            }

            var error = await Errors.TryTask(RequestTask());
            if (Errors.IsOccurred(error))
            {
                return (string.Empty, error);
            }

            return (message, Errors.Nothing());
        }


        public Action<IPsRequest> OnStartRequest { get; set; }

        private IErrors ValidNetworkAccess()
        {
            return _networkAccess.IsValid() ? Errors.Nothing() : Errors.New<NetworkInformationException>();
        }

        private HttpClient CreateClient(IPsWebRequestConfig config)
        {
            var client = _httpClientFactory.Create();
            client.Timeout = config.Timeout;
            return client;
        }

        public Action<bool> OnChangeRequesting { get; set; }
    }
}