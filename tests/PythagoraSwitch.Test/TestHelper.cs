using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using konnta0.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PythagoraSwitch.WebRequest;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.Test
{
    public class TestHelper
    {
        public static Mock<INetworkAccess> CreateNetworkAccessMock()
        {
            var networkAccess = new Mock<INetworkAccess>();
            networkAccess.Setup(x => x.IsValid()).Returns(true);
            return networkAccess;
        }

        public static Mock<IConfig> CreateConfigMock()
        {
            var config = new Mock<IConfig>();
            config.Setup(x => x.Timeout).Returns(new TimeSpan(0, 0, 30));
            config.Setup(x => x.QueueWatchDelayMilliseconds).Returns(20);
            config.Setup(x => x.RetryCount).Returns(1);
            config.Setup(x => x.RetryHttpStatusCodes).Returns(new HttpStatusCode[]
            {
                HttpStatusCode.TooManyRequests,
                HttpStatusCode.ServiceUnavailable
            });
            config.Setup(x => x.RetrySleepDurationProvider)
                .Returns(retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            config.Setup(x => x.Headers).Returns(new List<KeyValuePair<string, List<string>>>(
                new[]
                {
                    new KeyValuePair<string, List<string>>("Authorization", new List<string> {"Bearer Test"}),
                }));
            return config;
        }

        public static Mock<IRequestQueue> CreateRequestQueueMock(CancellationToken token)
        {
            var requestQueue = new Mock<IRequestQueue>();
            var queue = new Queue<Task>();
            async Task Action(int delay, Action<Task> callback, CancellationToken t)
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(delay, token);
                    if (queue.Count == 0) continue;
                    callback(requestQueue.Object.Dequeue());
                }
            }
            requestQueue.Setup(
                    x => x.WatchRequestQueue(It.IsAny<int>(),
                        It.IsAny<Action<Task>>(),
                        It.IsAny<CancellationToken>()))
                .Callback<int, Action<Task>, CancellationToken>(async (i, act, t) =>
                {
                    _ = await Errors.TryTask(Action(i, act, t));
                });
            requestQueue.Setup(x => x.Enqueue(It.IsAny<Task>())).Callback<Task>(request =>
            {
                queue.Enqueue(request);
            });
            requestQueue.Setup(x => x.Dequeue()).Returns(() => queue.Dequeue());
            return requestQueue;
        }

        public interface IHttpMessageHandler
        {
            Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
        }

        public static Mock<IHttpClientFactory> CreateHttpClientFactoryMock(HttpMethod httpMethod, string pathAndQuery)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .As<IHttpMessageHandler>()
                .Setup(m => m.SendAsync(
                    It.Is<HttpRequestMessage>(r =>
                        r.RequestUri.PathAndQuery.Contains(pathAndQuery) &&
                        r.Method == httpMethod
                    ),
                    It.IsAny<CancellationToken>()))
                .Returns(() =>
                {
                    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("{\"hoge\":\"hogehoge\"}")
                    };
                    return Task.FromResult(httpResponseMessage);
                });
            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(m => m.Create(It.IsAny<HttpClientHandler>()))
                .Returns(() => new HttpClient(handlerMock.Object, false));
            factoryMock.Setup(m => m.Create())
                .Returns(() => new HttpClient(handlerMock.Object, false));
            return factoryMock;
        }

        public static Mock<IWebRequestInterceptor> CreateWebRequestInterceptor()
        {
            var webRequestInterceptor = new Mock<IWebRequestInterceptor>();
            return webRequestInterceptor;
        }
    }
}