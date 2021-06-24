using System.Threading.Tasks;
using konnta0.Exceptions;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsRecorder
    {
        IErrors Start();
        IErrors Stop();
        IErrors Export(string outPath);
        IErrors Clear();
    }
}