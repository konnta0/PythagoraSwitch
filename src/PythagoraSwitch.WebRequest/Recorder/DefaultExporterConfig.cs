using PythagoraSwitch.WebRequest.Recorder.Interfaces;

namespace PythagoraSwitch.WebRequest.Recorder
{
    public class DefaultExporterConfig : IPsExporterConfig
    {
        public string FilePrefix => "recorded";

        public string OutPath => ".";
    }
}