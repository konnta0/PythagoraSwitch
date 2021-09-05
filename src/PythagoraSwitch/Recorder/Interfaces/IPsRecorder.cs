using konnta0.Exceptions;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsRecorder
    {
        IErrors Start();
        void Add(IPsRequestRecordContent content);
        IErrors Stop();
        (string, IErrors) Export();
        IErrors Clear();
    }
}