using PythagoraSwitch.Recorder.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public class DefaultExporterConfig : IPsExporterConfig
    {
        public string fileFormat { get; init; }
        public string outPath { get; init; }
    }
}