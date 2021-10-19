using System.Threading.Tasks;
using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IWebCommunication<in TReq, TRes> where TReq : IPsWebRequestContent  where TRes : IPsWebResponseContent
    {
        Task<(TRes, IErrors)> HandleRequest(TReq req);
    }
}
