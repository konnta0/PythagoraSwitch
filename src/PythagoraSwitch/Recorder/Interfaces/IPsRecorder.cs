using konnta0.Exceptions;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsRecorder
    {
        IErrors Start();
        void Add(IRequestRecordContent content);
        IErrors Stop();
        (string, IErrors) Export();
        IErrors Clear();
    }
}