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
        public static Mock<IPsNetworkAccess> CreateNetworkAccessMock()
        {
            var networkAccess = new Mock<IPsNetworkAccess>();
            networkAccess.Setup(x => x.IsValid()).Returns(true);
            return networkAccess;
        }

        public static Mock<IPsConfig> CreateConfigMock()
        {
            var config = new Mock<IPsConfig>();
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
            config.Setup(x => x.Headers).Returns(new List<KeyValuePair<string, IEnumerable<string>>>(
                new[]
                {
                    new KeyValuePair<string, IEnumerable<string>>("Authorization", new string[] {"Bearer Test"}),
                }));
            return config;
        }

        public static Mock<ILogger<PsWebRequester>> CreateLoggerMock()
        {
            return new();
        }

        public static Mock<IPsRequestQueue> CreatePsRequestQueueMock(CancellationToken token)
        {
            var requestQueue = new Mock<IPsRequestQueue>();
            var queue = new Queue<IPsRequest>();
            async Task Action(int delay, Action<IPsRequest> callback)
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(delay, token);
                    if (queue.Count == 0) continue;
                    callback(requestQueue.Object.Dequeue());
                }
            }
            requestQueue.Setup(x => x.WatchRequestQueue(It.IsAny<int>(), It.IsAny<Action<IPsRequest>>(), CancellationToken.None))
                .Callback<int, Action<IPsRequest>>(async (i, act) =>
                {
                    _ = await Errors.TryTask(Action(i, act));
                });
            requestQueue.Setup(x => x.Enqueue(It.IsAny<IPsRequest>())).Callback<IPsRequest>(request =>
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

        public static Mock<IPsHttpClientFactory> CreatePsHttpClientFactoryMock(HttpMethod httpMethod, string pathAndQuery)
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
            var factoryMock = new Mock<IPsHttpClientFactory>();
            factoryMock.Setup(m => m.Create(It.IsAny<HttpClientHandler>()))
                .Returns(() => new HttpClient(handlerMock.Object, false));
            factoryMock.Setup(m => m.Create())
                .Returns(() => new HttpClient(handlerMock.Object, false));
            return factoryMock;
        }
    }
}