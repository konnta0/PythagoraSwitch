using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;
using konnta0.Exceptions;
using Microsoft.Extensions.Logging;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public class PsWebRequester : IPsWebRequester, IPsWebRequesting
    {
        private readonly ILogger _logger;
        private readonly IPsNetworkAccess _networkAccess;
        private readonly IPsConfig _config;

        private readonly Queue<Request> _requestQueue;
        public bool Doing { get; private set; }

        public PsWebRequester(ILogger logger, IPsNetworkAccess networkAccess, IPsConfig config)
        {
            _logger = logger;
            _networkAccess = networkAccess;
            _config = config;
            _requestQueue = new Queue<Request>();
            WatchRequestQueue();
        }

        private async void WatchRequestQueue()
        {
            while (true)
            {
                if (_requestQueue.Count == 0)
                {
                    await Task.Delay(20);
                    continue;
                }

                OnChangeRequesting?.Invoke(Doing = true);
                var request = _requestQueue.Dequeue();
                var (responseMessage, error) = await request.HandleTask;
                request.OnResponse((responseMessage, error));
                OnChangeRequesting?.Invoke(Doing = false);
            }
        }

        private class Request
        {
            public Task<(string, IErrors)> HandleTask { get; set; }
            public Action<(string, IErrors)> OnResponse { get; set; }
        }
        
        public async Task<(TRes, IErrors)> PostAsync<TReq, TRes>(string url, TReq body) 
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
                       (httpResponse, error) = HandleResponse<TRes>(responseMessage);
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

        private async Task<(string, IErrors)> RequestPostTask<TReq>(string url, TReq body)
            where TReq : IPsWebPostRequestContent
        {
            var validNetworkAccess = ValidNetworkAccess();
            if (validNetworkAccess != null)
            {
                return (default, validNetworkAccess);
            }

            var message = string.Empty;
            async Task<IErrors> RequestTask()
            {
                var jsonObject = JsonSerializer.Serialize(body);
                var content = new StringContent(jsonObject);
                _logger.LogInformation($"[Http] REQUEST method:POST url:{url} body:{jsonObject}");
                using var client = CreateClient();
                using var responseMessage = await client.PostAsync(url, content);
                _logger.LogInformation(
                    $"[Http] RESPONSE method:POST url:{url} statusCode:{responseMessage.StatusCode}");
                if (!responseMessage.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"[Http] RESPONSE method:POST url:{url} statusCode:{responseMessage.StatusCode}");
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

        private async Task<(string, IErrors)> RequestGetTask<TGetReq>(string url, TGetReq queryObject)
            where TGetReq : IPsWebGetRequestContent
        {
            var validNetworkAccess = ValidNetworkAccess();
            if (validNetworkAccess != null)
            {
                return (default, validNetworkAccess);
            }
            var requestUrl = $"{url}&{queryObject.ToQueryString()}";

            _logger.LogInformation($"[Http] REQUEST method:GET url:{requestUrl}");
            var message = string.Empty;
            async Task<IErrors> RequestTask()
            {
                using var client = CreateClient();
                using var responseMessage = await client.GetAsync(requestUrl);
                _logger.LogInformation(
                    $"[Http] RESPONSE method:GET url:{requestUrl} statusCode:{responseMessage.StatusCode}");
                if (!responseMessage.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        $"[Http] RESPONSE failed method:GET url:{requestUrl} statusCode:{responseMessage.StatusCode}");
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

        public async Task<(TRes, IErrors)> GetAsync<TGetReq, TRes>(string url, TGetReq queryObject)
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
                        (httpResponse, error) = HandleResponse<TRes>(responseMessage);
                    }
                    isDone = true;  
                }
            };

            _requestQueue.Enqueue(request);
            while (!isDone)
            {
                await Task.Delay(100);
            }

            return (httpResponse, error);
        }

        private (TRes, IErrors) HandleResponse<TRes>(string message)
            where TRes : IPsWebResponseContent
        {
            TRes httpResponse = default;
            
            var error =  Errors.Try(() =>
            {
                _logger.LogInformation($"[Http] RESPONSE content: {message}");
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                httpResponse = JsonSerializer.Deserialize<TRes>(message, options);
            });
            return (httpResponse, error);
        }

        // This method must be in a class in a platform project, even if
        // the HttpClient object is constructed in a shared project.
        HttpClientHandler GetInsecureHandler()
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };;
        }

        private IErrors ValidNetworkAccess()
        {
            return !_networkAccess.IsValid() ? Errors.Nothing() : Errors.New<NetworkInformationException>();
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient();;
#if DEBUG
            var insecureHandler = GetInsecureHandler();

            client = new HttpClient(insecureHandler);
#endif
            client.Timeout = _config.Timeout;
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