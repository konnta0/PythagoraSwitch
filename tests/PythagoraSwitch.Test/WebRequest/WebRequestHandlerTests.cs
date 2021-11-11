using System;
using System.Threading;
using System.Threading.Tasks;
using konnta0.Exceptions;
using Moq;
using PythagoraSwitch.WebRequest;
using Xunit;

namespace PythagoraSwitch.Test.WebRequest
{
    public class WebRequestHandlerTests
    {
        [Fact(Timeout = 300)]
        public async void SimpleGetRequestTest()
        {
            var logger = LoggerFactory.Create<WebRequestHandler>();
            var networkAccess = TestHelper.CreateNetworkAccessMock();
            var config = TestHelper.CreateConfigMock();
            using var tokenSource = new CancellationTokenSource();
            var requestQueue = TestHelper.CreateRequestQueueMock(tokenSource.Token);
            var interceptor = TestHelper.CreateWebRequestInterceptor();
            var dummyResponse = new DummyGetResponseContent
            {
                hoge = "hogehoge"
            };
            interceptor.Setup(m => 
                    m.Handle(It.IsAny<RequestInfo>(),
                        It.IsAny<Func<RequestInfo, Task<(DummyGetResponseContent, IErrors)>>>()))
                .ReturnsAsync(() => (dummyResponse, Errors.Nothing()));
            var handler = new WebRequestHandler(
                logger,
                networkAccess.Object,
                config.Object,
                requestQueue.Object,
                interceptor.Object
            );
            var (response, error) =
                await handler.GetAsync<DummyGetRequestContent, DummyGetResponseContent>(
                    new Uri("http://pstest/api/dummy/get"), new DummyGetRequestContent());
            tokenSource.Cancel();
            Assert.False(Errors.IsOccurred(error),
                $"message: {error?.Exception.Message} \n trace: {error?.Exception.StackTrace}");
            Assert.NotNull(response);
            Assert.Equal("hogehoge", response.hoge);
        }

        [Fact(Timeout = 300)]
        public async void SimplePostRequestTest()
        {
            var logger = LoggerFactory.Create<WebRequestHandler>();
            var networkAccess = TestHelper.CreateNetworkAccessMock();
            var config = TestHelper.CreateConfigMock();
            using var tokenSource = new CancellationTokenSource();
            var requestQueue = TestHelper.CreateRequestQueueMock(tokenSource.Token);
            var interceptor = TestHelper.CreateWebRequestInterceptor();
            var dummyResponse = new DummyPostResponseContent
            {
                hoge = "hogehoge"
            };
            interceptor.Setup(m =>
                m.Handle(It.IsAny<RequestInfo>(),
                    It.IsAny<Func<RequestInfo, Task<(DummyPostResponseContent, IErrors)>>>()))
                                .ReturnsAsync(() => (dummyResponse, Errors.Nothing()));
            
            var handler = new WebRequestHandler(
                logger,
                networkAccess.Object,
                config.Object,
                requestQueue.Object,
                interceptor.Object
            );
            var (response, error) =
                await handler.PostAsync<DummyPostRequestContent, DummyPostResponseContent>(
                    new Uri("http://pstest/api/dummy/post"), new DummyPostRequestContent());
                
            tokenSource.Cancel();
            Assert.False(Errors.IsOccurred(error),
                $"message: {error?.Exception.Message} \n trace: {error?.Exception.StackTrace}");
            Assert.NotNull(response);
            Assert.Equal("hogehoge", response.hoge);
        }
    }
}