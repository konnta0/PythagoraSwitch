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
    public class PsWebRequest
    {
        [Fact]
        public async void SimpleRequestTest()
        {
            var psHttpClientFactoryMock = TestHelper.CreatePsHttpClientFactoryMock();
            
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
    }
}