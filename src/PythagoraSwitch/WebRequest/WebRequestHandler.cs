using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using konnta0.Exceptions;
using Microsoft.Extensions.Logging;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public sealed class WebRequestHandler : IWebRequestHandler
    {
        private readonly ILogger<WebRequestHandler> _logger;
        private readonly IPsNetworkAccess _networkAccess;
        private readonly IPsConfig _config;
        private readonly IPsRequestQueue _requestQueue;
        private readonly IWebRequestInterceptor _requestInterceptor;
        
        public WebRequestHandler(
            ILogger<WebRequestHandler> logger,
            IPsNetworkAccess networkAccess,
            IPsConfig config,
            IPsRequestQueue requestQueue,
            IWebRequestInterceptor requestInterceptor)
        {
            _logger = logger;
            _networkAccess = networkAccess;
            _config = config;
            _requestQueue = requestQueue;
            _requestInterceptor = requestInterceptor;

            var tokenSource = new CancellationTokenSource();
            _requestQueue.WatchRequestQueue(_config.QueueWatchDelayMilliseconds, HandleRequest, tokenSource.Token);
        }

        private async void HandleRequest(Task requestTask)
        {
            await requestTask;
        }
        
        public async Task<(TRes, IErrors)> PostAsync<TReq, TRes>(Uri uri, TReq body, IOption option = null) 
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

            var interceptors = new RequestInterceptors();
            if (option != null)
            {
                interceptors.AddRange(option.RequestInterceptors);
            }
            interceptors.Add(_requestInterceptor);
            
            async Task RequestTask()
            {
                var (response, interceptError) = await interceptors.Intercept(new RequestInfo
                {
                    Config = _config,
                    Content = body,
                    Headers = _config.Headers,
                    Method = HttpMethod.Post,
                    Uri = uri,
                    ContentType = typeof(TReq)
                });
                
                isDone = true;

                error = interceptError;
                if (Errors.IsOccurred(error)) return;
                httpResponse = (TRes)response;
            }
            
            _requestQueue.Enqueue(RequestTask());
            while (!isDone)
            {
                await Task.Delay(50);
            }
            return (httpResponse, error);
        }

        public async Task<(TRes, IErrors)> GetAsync<TGetReq, TRes>(Uri uri, TGetReq queryObject, IOption option = null)
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
            
            var interceptors = new RequestInterceptors();
            if (option != null)
            {
                interceptors.AddRange(option.RequestInterceptors);
            }
            interceptors.Add(_requestInterceptor);

            async Task RequestTask()
            {
                var (response, interceptError) = await interceptors.Intercept(new RequestInfo
                {
                    Config = _config,
                    Content = queryObject,
                    Headers = _config.Headers,
                    Method = HttpMethod.Get,
                    Uri = uri, 
                    ContentType = typeof(TGetReq)
                });
                
                isDone = true;

                error = interceptError;
                if (Errors.IsOccurred(error)) return;
                httpResponse = (TRes)response;
            }
            
            _requestQueue.Enqueue(RequestTask());
            
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
    }
}