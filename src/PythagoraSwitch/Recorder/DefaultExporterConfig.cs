using PythagoraSwitch.WebRequest.Recorder.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public sealed class DefaultExporterConfig : IPsExporterConfig
    {
        public string FilePrefix => "recorded";

        public string OutPath => ".";
    }
}