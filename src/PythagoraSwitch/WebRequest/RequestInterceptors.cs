using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public class RequestInterceptors : IRequestInterceptors
    {
        private readonly List<IWebRequestInterceptor> _interceptors;

        public RequestInterceptors()
        {
            _interceptors = new List<IWebRequestInterceptor>();
        }

        public void Add<T>() where T : IWebRequestInterceptor, new()
        {
            Add(new T());    
        }

        public void Add<T>(T interceptor) where T : IWebRequestInterceptor
        {
            _interceptors.Add(interceptor);
            if (_interceptors.Count != 1)
            {
                _interceptors[^2].NextFunc = _interceptors.Last().Handle;
            }
        }
        
        public void AddRange(List<IWebRequestInterceptor> interceptors)
        {
            interceptors.ForEach(Add);
        }

        public void AddRange(RequestInterceptors requestInterceptors)
        {
            AddRange(requestInterceptors._interceptors);
        }

        public async Task<(IWebResponseContent, IErrors)> Intercept(RequestInfo requestInfo)
        {
            if (_interceptors.Count == 0)
            {
                return (default, Errors.New("interceptor is empty."));
            }

            return await _interceptors.First().Handle(requestInfo);
        }

        internal int Count => _interceptors.Count;
    }
}