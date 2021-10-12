using System.Collections.Generic;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public class Option
    {
        public RequestInterceptors RequestInterceptors { get; } = new RequestInterceptors();
    }

    public class RequestInterceptors
    {
        private List<IWebRequestInterceptor> _interceptors;

        public RequestInterceptors()
        {
            _interceptors = new List<IWebRequestInterceptor>();
        }

        public void Add<T>(T interceptor) where T : IWebRequestInterceptor
        {
            _interceptors.Add(interceptor);
        }
        

        public void AddRange(List<IWebRequestInterceptor> interceptors)
        {
            _interceptors.AddRange(interceptors);
        }
        
        internal void ApplyChain(IPsWebRequestContent request)
        {
            for (var i = 0; i < _interceptors.Count; i++)
            {
                if (i + 1 < _interceptors.Count)
                {
                    _interceptors[i].NextFunc = _interceptors[i + 1].Handle;
                }
            }       
        }
    }
}