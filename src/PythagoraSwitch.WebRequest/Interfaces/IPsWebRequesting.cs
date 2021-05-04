using System;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IPsWebRequesting : IPsDoing
    {
        Action<bool> OnChangeRequesting { get; set; }
    }
}