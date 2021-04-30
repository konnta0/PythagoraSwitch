using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using konnta0.Exceptions;
using Microsoft.Extensions.Logging;
using Polly;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public class PsWebRequester : IPsWebRequester, IPsWebRequesting
    {
        private readonly ILogger<PsWebRequester> _logger;
        private readonly IPsNetworkAccess _networkAccess;
        private readonly IPsConfig _config;
        private readonly IPsSerializer _serializer;
        private readonly IPsRequestQueue _requestQueue;
        private readonly IPsHttpClientFactory _httpClientFactory;
        
        public bool Doing { get; private set; }

        public PsWebRequester(ILogger<PsWebRequester> logger, IPsNetworkAccess networkAccess, IPsHttpClientFactory httpClientFactory) : this(logger, networkAccess, new PsDefaultConfig(), new PsJsonSerializer(), new PsRequestQueue(), httpClientFactory)
        {
        }

        public PsWebRequester(ILogger<PsWebRequester> logger, IPsNetworkAccess networkAccess, IPsConfig config, IPsSerializer serializer, IPsRequestQueue requestQueue, IPsHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _networkAccess = networkAccess;
            _config = config;
            _serializer = serializer;
            _requestQueue = requestQueue;
            _httpClientFactory = httpClientFactory;
            _requestQueue.WatchRequestQueue(_config.QueueWatchDelayMilliseconds, HandleRequest);
        }

        private async void HandleRequest(IPsRequest request)
        {
            OnChangeRequesting?.Invoke(Doing = true);
            var (responseMessage, error) = await request.HandleTask;
            request.OnResponse((responseMessage, error));
            OnChangeRequesting?.Invoke(Doing = false);
        }

        public async Task<(TRes, IErrors)> PostAsync<TReq, TRes>(string url, TReq body, IPsWebRequestConfig overwriteConfig = null) 
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
                HandleTask = RequestPostTask(url, body),
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

        private async Task<(string, IErrors)> RequestPostTask<TReq>(string url, TReq body, IPsWebRequestConfig overwriteConfig = null)
            where TReq : IPsWebPostRequestContent
        {
            var validNetworkAccess = ValidNetworkAccess();
            if (validNetworkAccess != null)
            {
                return (default, validNetworkAccess);
            }
            var requestConfig = overwriteConfig ?? _config; 

            var message = string.Empty;
            async Task<IErrors> RequestTask()
            {
                var (str, serializedError) = _serializer.Serialize(body);
                if (Errors.IsOccurred(serializedError))
                {
                    return serializedError;
                }
                _logger.LogInformation("[Http] REQUEST method:POST url:{Url}", url);
                var client = CreateClient(requestConfig);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url) {Content = new StringContent(str)};

                using var responseMessage = await Policy
                    .HandleResult<HttpResponseMessage>(x => requestConfig.RetryHttpStatusCodes.Contains(x.StatusCode))
                    .WaitAndRetryAsync(requestConfig.RetryCount, requestConfig.RetrySleepDurationProvider)
                    .ExecuteAsync(() => client.SendAsync(requestMessage));
                _logger.LogInformation("[Http] RESPONSE method:POST url:{Url} statusCode:{Status}", url, responseMessage.StatusCode);
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

        private async Task<(string, IErrors)> RequestGetTask<TGetReq>(string url, TGetReq queryObject, IPsWebRequestConfig overwriteConfig = null)
            where TGetReq : IPsWebGetRequestContent
        {
            var validNetworkAccess = ValidNetworkAccess();
            if (validNetworkAccess != null)
            {
                return (default, validNetworkAccess);
            }

            var requestConfig = overwriteConfig ?? _config; 
            var requestUrl = $"{url}&{queryObject.ToQueryString()}";

            _logger.LogInformation("[Http] REQUEST method:GET url:{Url}", requestUrl);
            var message = string.Empty;
            async Task<IErrors> RequestTask()
            {
                var client = CreateClient(requestConfig);
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);

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

        public async Task<(TRes, IErrors)> GetAsync<TGetReq, TRes>(string url, TGetReq queryObject, IPsWebRequestConfig overwriteConfig = null)
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
                HandleTask = RequestGetTask(url, queryObject),
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
    
    public static class UrlHelpers
    {
        public static string ToQueryString(this IPsWebGetRequestContent request, string separator = ",")
        {
            if (request == null)
                throw new ArgumentNullException("request");

            // Get all properties on the object
            var properties = request.GetType().GetProperties()
                .Where(x => x.CanRead)
                .Where(x => x.GetValue(request, null) != null)
                .ToDictionary(x => x.Name, x => x.GetValue(request, null));

            // Get names for all IEnumerable properties (excl. string)
            var propertyNames = properties
                .Where(x => !(x.Value is string) && x.Value is IEnumerable)
                .Select(x => x.Key)
                .ToList();

            // Concat all IEnumerable properties into a comma separated string
            foreach (var key in propertyNames)
            {
                var valueType = properties[key].GetType();
                var valueElemType = valueType.IsGenericType
                    ? valueType.GetGenericArguments()[0]
                    : valueType.GetElementType();
                if (valueElemType.IsPrimitive || valueElemType == typeof(string))
                {
                    var enumerable = properties[key] as IEnumerable;
                    properties[key] = string.Join(separator, enumerable.Cast<object>());
                }
            }

            // Concat all key/value pairs into a string separated by ampersand
            return string.Join("&", properties
                .Select(x => string.Concat(
                    Uri.EscapeDataString(x.Key), "=",
                    Uri.EscapeDataString(x.Value.ToString()))));
        }
    }    
}