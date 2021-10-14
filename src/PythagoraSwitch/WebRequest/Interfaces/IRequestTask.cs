using System.Threading.Tasks;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IRequestTask
    {
        Task Handle { get; set; }
    }
}