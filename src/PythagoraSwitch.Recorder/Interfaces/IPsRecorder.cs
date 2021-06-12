using System.Threading.Tasks;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsRecorder
    {
        ValueTask Start();
        ValueTask Stop();
        ValueTask Export();
        ValueTask Clear();
    }
}