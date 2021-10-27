using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using konnta0.Exceptions;
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
            public TimeSpan Timeout { get; }
            public int RetryCount { get; }
            public HttpStatusCode[] RetryHttpStatusCodes { get; }
            public Func<int, TimeSpan> RetrySleepDurationProvider { get; }
            public List<KeyValuePair<string, List<string>>> Headers { get; }

            public Config()
            {
            }

            public Config(TimeSpan timeout)
            {
                Timeout = timeout;
            }
        }
        
        [Fact]
        private void GetTest()
        {

        }

        [Fact]
        private void PostTest()
        {
            
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