using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public class EmptyNetworkAccess : INetworkAccess
    {
        public bool IsValid() => true;
    }
}