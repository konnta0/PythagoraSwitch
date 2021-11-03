using System.Collections.Generic;
using System.Threading.Tasks;
using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IRequestInterceptors
    {
        void Add<T>(T interceptor) where T : IWebRequestInterceptor;
        void AddRange(List<IWebRequestInterceptor> interceptors);
        void AddRange(RequestInterceptors requestInterceptors);

        Task<(TRes, IErrors)> Intercept<TRes>(RequestInfo requestInfo) where TRes : IWebResponseContent;
    }
}