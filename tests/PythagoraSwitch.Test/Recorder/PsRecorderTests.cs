using konnta0.Exceptions;
using PythagoraSwitch.Recorder;
using Xunit;

namespace PythagoraSwitch.Test.Recorder
{
    public class PsRecorderTests
    {
        [Fact]
        internal void Start()
        {
            var recorder = new PsRecorder(new PsExporter(new DefaultExporterConfig(), LoggerFactory.Create<PsExporter>()));
            Assert.Equal(Errors.Nothing(), recorder.Start());

            // already started
            var error = recorder.Start();
            Assert.True(Errors.IsOccurred(error));
            Assert.Equal("already staring. please call Stop()", error.Exception.Message);
        }

        [Fact]
        internal void Stop()
        {
            var recorder = new PsRecorder(new PsExporter(new DefaultExporterConfig(), LoggerFactory.Create<PsExporter>()));

            // not started
            var error = recorder.Stop();
            Assert.True(Errors.IsOccurred(error));
            Assert.Equal("already stopped. please call Start()", error.Exception.Message);

            _ = recorder.Start();
            Assert.Equal(Errors.Nothing(), recorder.Stop());
        }
    }
}
