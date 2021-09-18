using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.Recorder.Interfaces;

namespace PythagoraSwitch.Player.Interfaces
{
    public interface IPsPlayer
    {
        Task<IErrors> Handle<T>(string path) where T : IPsRequestRecordContent;
    }
}