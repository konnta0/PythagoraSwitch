using PythagoraSwitch.Recorder.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public sealed class DefaultExporterConfig : IExporterConfig
    {
        public string FilePrefix => "recorded";

        public string OutPath => ".";
    }
}