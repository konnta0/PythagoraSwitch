using System;
using System.Threading.Tasks;
using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IWebRequestHandler
    {
        Task<(TRes, IErrors)> PostAsync<TReq, TRes>(Uri uri, TReq body, IOption option = null) 
            where TReq : IWebPostRequestContent where TRes : IWebResponseContent; 
        
        Task<(TRes, IErrors)> GetAsync<TGetReq, TRes>(Uri uri, TGetReq queryObject, IOption option = null)
            where TGetReq : IWebGetRequestContent where TRes : IWebResponseContent;
    }
}