using System;
using System.Net.Http;
using System.Threading;
using konnta0.Exceptions;
using Moq;
using PythagoraSwitch.Player;
using PythagoraSwitch.Player.Interfaces;
using PythagoraSwitch.Recorder;
using PythagoraSwitch.WebRequest;
using Xunit;

namespace PythagoraSwitch.Test.Importer
{
    public class PsPlayerTests
    {
        [Fact(Timeout = 300)]
        public async void Handle()
        {
            var psHttpClientFactoryMock = TestHelper.CreatePsHttpClientFactoryMock(HttpMethod.Get, "api/dummy/path");
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

            var importer = new Mock<IPsImporter>();
            importer.Setup(x => x.Handle<PsRequestRecordContent>(It.IsAny<string>()))
                .Returns(() => (DummyRecordFactory.Create(), Errors.Nothing()));
            var player = new PsPlayer(requester, importer.Object);

            var error = await player.Handle<PsRequestRecordContent>("dummy");

            tokenSource.Cancel();
            Assert.False(Errors.IsOccurred(error));
        }
    }
}