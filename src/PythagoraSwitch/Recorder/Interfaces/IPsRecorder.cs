using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Recorder.Interfaces
{
    public interface IPsRecorder
    {
        IErrors Start();
        IErrors Stop();
        IErrors Export();
        IErrors Clear();
    }
}