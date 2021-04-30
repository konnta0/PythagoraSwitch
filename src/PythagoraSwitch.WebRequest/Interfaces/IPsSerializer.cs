using System.ComponentModel;
using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IPsSerializer
    {
        string ContentType { get; }
        (string, IErrors) Serialize<TReq>(TReq req) where TReq : IPsWebRequestContent;
        (TRes, IErrors) Deserialize<TRes>(string message) where TRes : IPsWebResponseContent;
    }
}