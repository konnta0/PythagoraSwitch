using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    internal class RequestInterceptor : IWebRequestInterceptor
    {
        public Func<RequestInfo, Task<(IPsWebResponseContent, IErrors)>> NextFunc { get; set; }

        private readonly IPsNetworkAccess _networkAccess;
        private readonly IPsSerializer _serializer;

        public RequestInterceptor(IPsSerializer serializer, IPsNetworkAccess networkAccess)
        {
            _serializer = serializer;
            _networkAccess = networkAccess;
        }

        public async Task<(IPsWebResponseContent, IErrors)> Handle(RequestInfo content)
        {
            var validNetworkAccess = ValidNetworkAccess();
            if (validNetworkAccess != null)
            {
                return (default, validNetworkAccess);
            }
            return await NextFunc(content);
        }
        
        private IErrors ValidNetworkAccess()
        {
            return _networkAccess.IsValid() ? Errors.Nothing() : Errors.New<NetworkInformationException>();
        }
    }
}