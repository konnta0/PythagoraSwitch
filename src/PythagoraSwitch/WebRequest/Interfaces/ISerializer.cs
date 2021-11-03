using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface ISerializer
    {
        string ContentType { get; }
        (string, IErrors) Serialize<TReq>(TReq req) where TReq : IWebRequestContent;
        (TRes, IErrors) Deserialize<TRes>(string message) where TRes : IWebResponseContent;
    }
}