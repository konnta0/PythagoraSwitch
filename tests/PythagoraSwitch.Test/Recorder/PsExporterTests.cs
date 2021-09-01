using konnta0.Exceptions;
using PythagoraSwitch.Recorder;
using PythagoraSwitch.WebRequest.Recorder;
using Xunit;

namespace PythagoraSwitch.Test.Recorder
{
    public class PsExporterTests
    {
        [Fact]
        internal void HandleTest()
        {
            var exporter = new PsExporter(new DefaultExporterConfig(), LoggerFactory.Create<PsExporter>());
            var recordContents = DummyRecordFactory.Create();
            var (path, errors) = exporter.Handle(recordContents);
            Assert.False(Errors.IsOccurred(errors));
        }
    }
}