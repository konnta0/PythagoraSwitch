namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IExporterConfig
    {
        string FilePrefix { get; }
        string OutPath { get;}
    }
}