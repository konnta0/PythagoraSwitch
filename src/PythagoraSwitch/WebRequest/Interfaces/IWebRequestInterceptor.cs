
using System;
using System.Threading.Tasks;
using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IWebRequestInterceptor : IInterceptor
    {
        Func<RequestInfo, Task<(IWebResponseContent, IErrors)>> NextFunc { get; set; }

        Task<(TRes, IErrors)> Handle<TRes>(RequestInfo requestInfo, Func<RequestInfo, Task<(TRes, IErrors)>> next)
            where TRes : IWebResponseContent;
    }
}