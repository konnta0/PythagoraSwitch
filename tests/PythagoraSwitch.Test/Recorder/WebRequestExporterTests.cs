using System.IO;
using konnta0.Exceptions;
using PythagoraSwitch.Recorder;
using Xunit;
using Xunit.Abstractions;

namespace PythagoraSwitch.Test.Recorder
{
    public class WebRequestExporterTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WebRequestExporterTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        internal void HandleTest()
        {
            var exporter = new WebRequestExporter(new DefaultExporterConfig(), new ScenarioTemplate(), LoggerFactory.Create<WebRequestExporter>());
            var recordContents = DummyRecordFactory.CreateByInterface();
            var (path, errors) = exporter.Handle(recordContents);
            Assert.False(Errors.IsOccurred(errors));
        }

        [Fact]
        internal void HandleGenerateScenarioTest()
        {
            var exporter = new WebRequestExporter(new DefaultExporterConfig(), new ScenarioTemplate(), LoggerFactory.Create<WebRequestExporter>());
            var recordContents = DummyRecordFactory.CreateByInterface();
            var scenarioName = "DummyScenario";
            var (path, errors) = exporter.Handle(scenarioName, recordContents);
            if (Errors.IsOccurred(errors))
            {
                _testOutputHelper.WriteLine(errors.ToString());
            }
            Assert.False(Errors.IsOccurred(errors));
            Assert.Contains($"{scenarioName}.Generated.cs", path);
            _testOutputHelper.WriteLine(File.ReadAllText(path));
        }
    }
}