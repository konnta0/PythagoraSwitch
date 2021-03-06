using System;
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
    internal sealed class WebRequestInterceptor : IWebRequestInterceptor
    {
        public Func<RequestInfo, Task<(IWebResponseContent, IErrors)>> NextFunc { get; set; }

        private readonly ILogger<WebRequestInterceptor> _logger;
        private readonly INetworkAccess _networkAccess;
        private readonly ISerializer _serializer;
        private readonly IHttpClientFactory _httpClientFactory;

        public WebRequestInterceptor(ISerializer serializer, INetworkAccess networkAccess, IHttpClientFactory httpClientFactory, ILogger<WebRequestInterceptor> logger)
        {
            _serializer = serializer;
            _networkAccess = networkAccess;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<(TRes, IErrors)> Handle<TRes>(RequestInfo requestInfo,  Func<RequestInfo, Task<(TRes, IErrors)>> next) where TRes : IWebResponseContent
        {
            var validNetworkAccess = ValidNetworkAccess();
            if (validNetworkAccess != null)
            {
                return (default, validNetworkAccess);
            }

            string contentString;
            if (requestInfo.Method == HttpMethod.Get)
            {
                var (message, errors) = await Errors.TryTask(RequestGetTask(requestInfo.Config, requestInfo.Uri, (IWebGetRequestContent)requestInfo.Content));
                if (Errors.IsOccurred(errors))
                {
                    return (default, errors);
                }

                contentString = message;
            } else if (requestInfo.Method == HttpMethod.Post)
            {
                var (message, errors) = await Errors.TryTask(RequestPostTask(requestInfo.Config, requestInfo.Uri, requestInfo.Content));
                if (Errors.IsOccurred(errors))
                {
                    return (default, errors);
                }
                contentString = message;
            }
            else
            {
                return (default, Errors.New($"Un supported method {requestInfo.Method}"));
            }
            
            return _serializer.Deserialize<TRes>(contentString);
        }
        
        internal IErrors ValidNetworkAccess()
        {
            return _networkAccess.IsValid() ? Errors.Nothing() : Errors.New<NetworkInformationException>();
        }

        internal HttpClient CreateClient(IWebRequestConfig config)
        {
            var client = _httpClientFactory.Create();
            client.Timeout = config.Timeout;
            return client;
        }
        
        internal async Task<(string, IErrors)> RequestGetTask(IWebRequestConfig config, Uri url, IWebGetRequestContent content)
        {
            var client = CreateClient(config);
            var requestUrl = $"{url}&{content.ToQueryString()}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            foreach (var (key, value) in config.Headers)
            {
                requestMessage.Headers.Add(key, value);
            }

            _logger.LogInformation("[Http] REQUEST method:GET url:{Url}", requestUrl);

            using var responseMessage = await Policy
                .HandleResult<HttpResponseMessage>(x => config.RetryHttpStatusCodes.Contains(x.StatusCode))
                .WaitAndRetryAsync(config.RetryCount, config.RetrySleepDurationProvider)
                .ExecuteAsync(() => client.SendAsync(requestMessage));
            _logger.LogInformation(
                "[Http] RESPONSE method:GET url:{Url} statusCode:{Status}", requestUrl, responseMessage.StatusCode);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return (string.Empty,
                    Errors.New(new Exception($"requestTask failed status code {responseMessage.StatusCode}")));
            }

            var message = await responseMessage.Content.ReadAsStringAsync();
            return (message, Errors.Nothing());
        }

        internal async Task<(string, IErrors)> RequestPostTask(IWebRequestConfig config, Uri requestUrl, IWebRequestContent content)
        {
            var (str, serializedError) = _serializer.Serialize(content);
            if (Errors.IsOccurred(serializedError))
            {
                return (string.Empty, serializedError);
            }

            _logger.LogInformation("[Http] REQUEST method:POST url:{Url}", requestUrl.ToString());
            var client = CreateClient(config);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(str, Encoding.UTF8, _serializer.ContentType)
            };
            foreach (var (headerKey, headerValues) in config.Headers)
            {
                requestMessage.Headers.Add(headerKey, headerValues);
            }

            using var responseMessage = await Policy
                .HandleResult<HttpResponseMessage>(x => config.RetryHttpStatusCodes.Contains(x.StatusCode))
                .WaitAndRetryAsync(config.RetryCount, config.RetrySleepDurationProvider)
                .ExecuteAsync(() => client.SendAsync(requestMessage));
            _logger.LogInformation("[Http] RESPONSE method:POST url:{Url} statusCode:{Status}", requestUrl.ToString(),
                responseMessage.StatusCode);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return (string.Empty, Errors.New($"requestTask failed status code {responseMessage.StatusCode}"));
            }

            var message = await responseMessage.Content.ReadAsStringAsync();
            return (message, Errors.Nothing());
        }
    }
}