
using System;
using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IPsRequestInterceptor : IPsInterceptor
    {
        Func<IPsWebRequestContent, (IPsWebResponseContent, IErrors)> NextFunc { get; set; }
        (IPsWebResponseContent, IErrors) Handle(IPsWebRequestContent content);
    }

    public interface IPsInterceptor
    {
    }
}