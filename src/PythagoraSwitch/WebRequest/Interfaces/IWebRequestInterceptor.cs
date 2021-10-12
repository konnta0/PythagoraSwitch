
using System;
using System.Threading.Tasks;
using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IWebRequestInterceptor : IInterceptor
    {
        Func<RequestInfo, Task<(IPsWebResponseContent, IErrors)>> NextFunc { get; set; }
        Task<(IPsWebResponseContent, IErrors)> Handle(RequestInfo content);
    }
}