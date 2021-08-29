using System.Threading.Tasks;
using konnta0.Exceptions;

namespace PythagoraSwitch.Player.Interfaces
{
    public interface IPsPlayer
    {
        Task<IErrors> Handle(string path);
    }
}