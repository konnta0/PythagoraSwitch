using System;
using System.Threading.Tasks;
using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IWebRequestHandler
    {
        Task<(TRes, IErrors)> PostAsync<TReq, TRes>(Uri uri, TReq body, IOption option = null) 
            where TReq : IPsWebPostRequestContent where TRes : IPsWebResponseContent; 
        
        Task<(TRes, IErrors)> GetAsync<TGetReq, TRes>(Uri uri, TGetReq queryObject, IOption option = null)
            where TGetReq : IPsWebGetRequestContent where TRes : IPsWebResponseContent;
    }
}