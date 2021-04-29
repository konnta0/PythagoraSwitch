using System.Net.Http;
using System.Threading;
using konnta0.Exceptions;
using Xunit;

namespace PythagoraSwitch.WebRequest.Test
{
    public class PsWebRequestTests
    {
        [Fact]
        public async void SimpleGetRequestTest()
        {
            var psHttpClientFactoryMock = TestHelper.CreatePsHttpClientFactoryMock(HttpMethod.Get, "/api/dummy/get");
            
            var loggerMock = TestHelper.CreateLoggerMock();
            var networkAccess = TestHelper.CreateNetworkAccessMock();
            var config = TestHelper.CreateConfigMock();
            using var tokenSource = new CancellationTokenSource();
            var requestQueue = TestHelper.CreatePsRequestQueueMock(tokenSource.Token);
            var requester = new PsWebRequester(loggerMock.Object, networkAccess.Object, config.Object, new PsJsonSerializer(), requestQueue.Object, psHttpClientFactoryMock.Object);
            var (response, error) = await requester.GetAsync<DummyGetRequestContent, DummyGetResponseContent>("http://pstest/api/dummy/get", new DummyGetRequestContent());
            tokenSource.Cancel();
            Assert.False(Errors.IsOccurred(error));
            Assert.NotNull(response);
            Assert.Equal("hogehoge", response.hoge);
        }
        
        [Fact]
        public async void SimplePostRequestTest()
        {
            var psHttpClientFactoryMock = TestHelper.CreatePsHttpClientFactoryMock(HttpMethod.Post, "/api/dummy/post");
            
            var loggerMock = TestHelper.CreateLoggerMock();
            var networkAccess = TestHelper.CreateNetworkAccessMock();
            var config = TestHelper.CreateConfigMock();
            using var tokenSource = new CancellationTokenSource();
            var requestQueue = TestHelper.CreatePsRequestQueueMock(tokenSource.Token);
            var requester = new PsWebRequester(loggerMock.Object, networkAccess.Object, config.Object, new PsJsonSerializer(), requestQueue.Object, psHttpClientFactoryMock.Object);
            var (response, error) = await requester.PostAsync<DummyPostRequestContent, DummyPostResponseContent>("http://pstest/api/dummy/post", new DummyPostRequestContent());
            tokenSource.Cancel();
            Assert.False(Errors.IsOccurred(error));
            Assert.NotNull(response);
            Assert.Equal("hogehoge", response.hoge);
        }
    }
}