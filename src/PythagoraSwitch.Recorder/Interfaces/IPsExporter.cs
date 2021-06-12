using System.Threading.Tasks;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsExporter
    {
        ValueTask Handle();
    }
}