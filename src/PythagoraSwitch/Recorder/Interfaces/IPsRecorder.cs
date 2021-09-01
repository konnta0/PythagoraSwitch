using konnta0.Exceptions;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsRecorder
    {
        IErrors Start();
        IErrors Stop();
        (string, IErrors) Export();
        IErrors Clear();
    }
}