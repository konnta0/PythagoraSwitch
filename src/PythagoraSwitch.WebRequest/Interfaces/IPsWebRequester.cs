using System.Threading.Tasks;
using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IPsWebRequester
    {
        Task<(TRes, IErrors)> PostAsync<TReq, TRes>(string url, TReq body) 
            where TReq : IPsWebPostRequestContent where TRes : IPsWebResponseContent; 
        Task<(TRes, IErrors)> GetAsync<TGetReq, TRes>(string url, TGetReq queryObject)
            where TGetReq : IPsWebGetRequestContent where TRes : IPsWebResponseContent;
    }
}