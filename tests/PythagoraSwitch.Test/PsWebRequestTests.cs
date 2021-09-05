using System;
using System.Net.Http;
using System.Threading;
using konnta0.Exceptions;
using Microsoft.Extensions.Logging;
using PythagoraSwitch.Recorder;
using PythagoraSwitch.WebRequest;
using Xunit;

namespace PythagoraSwitch.Test
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
            var requester = new PsWebRequester(
                loggerMock.Object,
                networkAccess.Object,
                config.Object,
                new PsJsonSerializer(),
                requestQueue.Object,
                psHttpClientFactoryMock.Object,
                new PsRecorder(new PsExporter(new DefaultExporterConfig(), LoggerFactory.Create<PsExporter>())));
            var (response, error) = await requester.GetAsync<DummyGetRequestContent, DummyGetResponseContent>(new Uri("http://pstest/api/dummy/get"), new DummyGetRequestContent());
            tokenSource.Cancel();
            Assert.False(Errors.IsOccurred(error), $"message: {error?.Exception.Message} \n trace: {error?.Exception.StackTrace}");
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
            var requester = new PsWebRequester(
                loggerMock.Object,
                networkAccess.Object,
                config.Object,
                new PsJsonSerializer(),
                requestQueue.Object,
                psHttpClientFactoryMock.Object,
                new PsRecorder(new PsExporter(new DefaultExporterConfig(), LoggerFactory.Create<PsExporter>()))
                );
            var (response, error) = await requester.PostAsync<DummyPostRequestContent, DummyPostResponseContent>(new Uri("http://pstest/api/dummy/post"), new DummyPostRequestContent());
            tokenSource.Cancel();
            Assert.False(Errors.IsOccurred(error), $"message: {error?.Exception.Message} \n trace: {error?.Exception.StackTrace}");
            Assert.NotNull(response);
            Assert.Equal("hogehoge", response.hoge);
        }
    }
}