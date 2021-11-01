using System.Linq;
using konnta0.Exceptions;
using PythagoraSwitch.Recorder;
using Xunit;

namespace PythagoraSwitch.Test.Recorder
{
    public class RecorderTests
    {
        [Fact]
        internal void StartTest()
        {
            var recorder = new PythagoraSwitch.Recorder.Recorder(new WebRequestExporter(new DefaultExporterConfig(), LoggerFactory.Create<WebRequestExporter>()));
            Assert.Equal(Errors.Nothing(), recorder.Start());

            // already started
            var error = recorder.Start();
            Assert.True(Errors.IsOccurred(error));
            Assert.Equal("already staring. please call Stop()", error.Exception.Message);
        }

        [Fact]
        internal void StopTest()
        {
            var recorder = new PythagoraSwitch.Recorder.Recorder(new WebRequestExporter(new DefaultExporterConfig(), LoggerFactory.Create<WebRequestExporter>()));

            // not started
            var error = recorder.Stop();
            Assert.True(Errors.IsOccurred(error));
            Assert.Equal("already stopped. please call Start()", error.Exception.Message);

            _ = recorder.Start();
            Assert.Equal(Errors.Nothing(), recorder.Stop());
        }

        [Fact]
        internal void AddTest()
        {
            var recorder = new PythagoraSwitch.Recorder.Recorder(new WebRequestExporter(new DefaultExporterConfig(), LoggerFactory.Create<WebRequestExporter>()));
            
            recorder.Add(new RequestRecordContent
            {
                EndPoint = "hoge"
            });
            Assert.Empty(recorder.RecordContents);
            
            Assert.Equal(Errors.Nothing(), recorder.Start());
            recorder.Add(new RequestRecordContent
            {
                EndPoint = "hoge"
            });
            Assert.Single(recorder.RecordContents);
            Assert.Equal("hoge", recorder.RecordContents.First().EndPoint);
            recorder.Add(new RequestRecordContent
            {
                EndPoint = "fuga"
            });
            Assert.Equal(2, recorder.RecordContents.Count);
            Assert.Equal("fuga", recorder.RecordContents.Last().EndPoint);
        }

        [Fact]
        internal void ClearTest()
        {
            var recorder = new PythagoraSwitch.Recorder.Recorder(new WebRequestExporter(new DefaultExporterConfig(), LoggerFactory.Create<WebRequestExporter>()));
            Assert.Equal(Errors.Nothing(), recorder.Start());

            for (var i = 0; i < 10; i++)
            {
                recorder.Add(new RequestRecordContent());
            }
            Assert.Equal(10, recorder.RecordContents.Count);

            recorder.Clear();
            
            Assert.Empty(recorder.RecordContents);
        }
    }
}
