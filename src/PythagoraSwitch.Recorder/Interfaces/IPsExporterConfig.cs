namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsExporterConfig
    {
        string fileFormat { get; init; }
        string outPath { get; init; }
    }
}