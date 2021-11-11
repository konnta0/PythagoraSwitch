using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using konnta0.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using PythagoraSwitch.WebRequest;
using PythagoraSwitch.WebRequest.Interfaces;
using Xunit;

namespace PythagoraSwitch.Test.WebRequest
{
    public class WebRequestInterceptorTests
    {
        internal class Config : IWebRequestConfig
        {
            public TimeSpan Timeout { get; } = TimeSpan.Zero;
            public int RetryCount { get; } = 0;
            public HttpStatusCode[] RetryHttpStatusCodes { get; } = Array.Empty<HttpStatusCode>();
            public Func<int, TimeSpan> RetrySleepDurationProvider { get; } = i => TimeSpan.Zero;

            public List<KeyValuePair<string, List<string>>> Headers { get; } = new();

            public Config()
            {
                
            }

            public Config(TimeSpan timeout)
            {
                Timeout = timeout;
            }
        }
        
        [Fact]
        private async void GetTest()
        {
            var httpClientFactory = TestHelper.CreateHttpClientFactoryMock(HttpMethod.Get, "path/to");
            var logger = LoggerFactory.Create<WebRequestInterceptor>();
            var webRequestInterceptor = new WebRequestInterceptor(new JsonSerializer(), new EmptyNetworkAccess(), httpClientFactory.Object, logger);
            var (message, errors) = await webRequestInterceptor.RequestGetTask(
                new Config(TimeSpan.FromSeconds(10)),
                new Uri("https://dummy.com/path/to"),
                new DummyGetRequestContent()
                );
            Assert.Equal(Errors.Nothing(), errors);
            Assert.Equal("{\"hoge\":\"hogehoge\"}", message);
        }

        [Fact]
        private async void PostTaskTest()
        {
            var httpClientFactory = TestHelper.CreateHttpClientFactoryMock(HttpMethod.Post, "path/to");
            var logger = LoggerFactory.Create<WebRequestInterceptor>();
            var webRequestInterceptor = new WebRequestInterceptor(new JsonSerializer(), new EmptyNetworkAccess(), httpClientFactory.Object, logger);
            var (message, errors) = await webRequestInterceptor.RequestPostTask(
                new Config(TimeSpan.FromSeconds(10)),
                new Uri("https://dummy.com/path/to"),
                new DummyPostRequestContent{foobar = 456});
            Assert.Equal(Errors.Nothing(), errors);
            Assert.Equal("{\"hoge\":\"hogehoge\"}", message);
        }

        [Fact]
        private void ValidNetworkAccessTest()
        { 
            var networkAccess = new Mock<INetworkAccess>();
            networkAccess.Setup(x => x.IsValid()).Returns(true);
            var webRequestInterceptor = new WebRequestInterceptor(null, networkAccess.Object, null, null);
            var errors = webRequestInterceptor.ValidNetworkAccess();
            Assert.Equal(Errors.Nothing(), errors);
        }

        [Fact]
        private void CreateClientTest()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(m => m.Create())
                .Returns(() => new HttpClient());
            var webRequestInterceptor = new WebRequestInterceptor(null, null, httpClientFactory.Object, null);
            var timeout = new TimeSpan(1234);
            var config = new Config(timeout);
            var client = webRequestInterceptor.CreateClient(config);
            Assert.Equal(timeout.Ticks, client.Timeout.Ticks);
        }
    }
}