using System.Collections.Generic;
using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public sealed class RequestInterceptors : IRequestInterceptors
    {
        private readonly Stack<IWebRequestInterceptor> _interceptors;

        public RequestInterceptors()
        {
            _interceptors = new Stack<IWebRequestInterceptor>();
        }

        public void Add<T>() where T : IWebRequestInterceptor, new()
        {
            Add(new T());    
        }

        public void Add<T>(T interceptor) where T : IWebRequestInterceptor
        {
            _interceptors.Push(interceptor);
        }
        
        public void AddRange(List<IWebRequestInterceptor> interceptors)
        {
            interceptors.ForEach(Add);
        }

        public void AddRange(RequestInterceptors requestInterceptors)
        {
            foreach (var interceptor in requestInterceptors._interceptors)
            {
                Add(interceptor);
            }
        }

        public async Task<(TRes, IErrors)> Intercept<TRes>(RequestInfo requestInfo) where TRes : IWebResponseContent
        {
            if (_interceptors.Count == 0)
            {
                return (default, Errors.New("interceptor is empty."));
            }

            var workInterceptors = new Stack<IWebRequestInterceptor>(_interceptors);
            var next = workInterceptors.Pop().Handle<TRes>(requestInfo, null);

            while (workInterceptors.TryPop(out var interceptor))
            {
                var next1 = next;
                next = interceptor.Handle(requestInfo, info => next1);
            }

            return await next;
        }

        internal int Count => _interceptors.Count;
    }
}