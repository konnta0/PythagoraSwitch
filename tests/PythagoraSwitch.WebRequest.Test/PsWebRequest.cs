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
using PythagoraSwitch.WebRequest.Interfaces;
using Xunit;

namespace PythagoraSwitch.WebRequest.Test
{
    public interface IHttpMessageHandler
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
    
    
    public class PsWebRequest
    {
        [Fact]
        public async void Test1()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .As<IHttpMessageHandler>()
                .Setup(m => m.SendAsync(
                    It.Is<HttpRequestMessage>(r =>
                        r.RequestUri.PathAndQuery.Contains("/api/dummy/get") &&
                        r.Method == HttpMethod.Get 
                        //&&
                        // r.Content.ReadAsStringAsync().Result.Contains("foobar") &&
                        // r.Content.ReadAsStringAsync().Result.Contains("P@ssw0rd")
                    ),
                    It.IsAny<CancellationToken>()))
                .Returns(() =>
                {
                    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    return Task.FromResult(httpResponseMessage);
                });

            var loggerMock = CreateLoggerMock();
            var networkAccess = CreateNetworkAccessMock();
            var config = CreateConfigMock();
            var requestQueue = new Mock<IPsRequestQueue>();
            var queue = new Queue<IPsRequest>();
            var running = true;
            requestQueue.Setup(x => x.WatchRequestQueue(It.IsAny<int>(), It.IsAny<Action<IPsRequest>>()))
                .Callback<int, Action<IPsRequest>>(async (delay, callback) =>
                {
                    while (running)
                    {
                        await Task.Delay(delay);
                        if (queue.Count == 0) continue;
                        requestQueue.Object.Dequeue();
                    }
                });
            requestQueue.Setup(x => x.Enqueue(It.IsAny<IPsRequest>())).Callback<IPsRequest>(request =>
            {
                queue.Enqueue(request);
            });
            requestQueue.Setup(x => x.Dequeue()).Returns(() => queue.Dequeue());
            var requester = new PsWebRequester(loggerMock.Object, networkAccess.Object, config.Object, new PsJsonSerializer(), requestQueue.Object);
            var (response, error) = await requester.GetAsync<DummyGetRequestContent, DummyGetResponseContent>("/api/dummy/get", new DummyGetRequestContent());
            Assert.False(Errors.IsOccurred(error));
            Assert.NotNull(response);
            running = false;
        }

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
            return config;
        }

        public static Mock<ILogger<PsWebRequester>> CreateLoggerMock()
        {
            return new();
        }
    }
}