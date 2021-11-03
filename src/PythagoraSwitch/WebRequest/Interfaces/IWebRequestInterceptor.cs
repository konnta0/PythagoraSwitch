
using System;
using System.Threading.Tasks;
using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IWebRequestInterceptor : IInterceptor
    {
        Task<(TRes, IErrors)> Handle<TRes>(RequestInfo requestInfo, Func<RequestInfo, Task<(TRes, IErrors)>> next)
            where TRes : IWebResponseContent;
    }
}