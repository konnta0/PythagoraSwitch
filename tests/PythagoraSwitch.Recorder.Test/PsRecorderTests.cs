using konnta0.Exceptions;
using Xunit;

namespace PythagoraSwitch.Recorder.Test
{
    public class PsRecorderTests
    {
        [Fact]
        public void Start()
        {
            var recorder = new PsRecorder(new PsExporter(new DefaultExporterConfig(), LoggerFactory.Create<PsExporter>()));
            Assert.Equal(Errors.Nothing(), recorder.Start());

            // already started
            var error = recorder.Start();
            Assert.True(Errors.IsOccurred(error));
            Assert.Equal("already staring. please call Stop()", error.Exception.Message);
        }
    }
}
