using konnta0.Exceptions;
using PythagoraSwitch.Recorder;
using Xunit;

namespace PythagoraSwitch.Test.Recorder
{
    public class ExporterTests
    {
        [Fact]
        internal void HandleTest()
        {
            var exporter = new WebRequestExporter(new DefaultExporterConfig(), LoggerFactory.Create<WebRequestExporter>());
            var recordContents = DummyRecordFactory.CreateByInterface();
            var (path, errors) = exporter.Handle(recordContents);
            Assert.False(Errors.IsOccurred(errors));
        }
    }
}