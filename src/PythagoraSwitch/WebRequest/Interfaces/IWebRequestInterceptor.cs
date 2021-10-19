
using System;
using System.Threading.Tasks;
using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IWebRequestInterceptor : IInterceptor
    {
        Func<RequestInfo, Task<(IWebResponseContent, IErrors)>> NextFunc { get; set; }
        Task<(IWebResponseContent, IErrors)> Handle(RequestInfo content);
    }
}