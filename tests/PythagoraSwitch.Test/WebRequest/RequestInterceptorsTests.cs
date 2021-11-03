using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.WebRequest;
using PythagoraSwitch.WebRequest.Interfaces;
using Xunit;

namespace PythagoraSwitch.Test.WebRequest
{
    public class RequestInterceptorsTests
    {
        internal class DummyInterceptor : IWebRequestInterceptor
        {
            public Task<(TRes, IErrors)> Handle<TRes>(RequestInfo requestInfo, Func<RequestInfo, Task<(TRes, IErrors)>> next) where TRes : IWebResponseContent
            {
                throw new NotImplementedException();
            }

            public async Task<(IWebResponseContent, IErrors)> Handle(RequestInfo content)
            {
                await Task.Delay(1);
                return (null, Errors.Nothing());
            }
        }

        [Fact]
        public void Add()
        {
            var interceptors = new RequestInterceptors();
            interceptors.Add<DummyInterceptor>();
            interceptors.Add<DummyInterceptor>();
            interceptors.Add(new DummyInterceptor());
            Assert.Equal(3, interceptors.Count);
        }

        [Fact]
        public void AddRange()
        {
            var interceptors = new RequestInterceptors();
            var interceptorList = new List<IWebRequestInterceptor>()
            {
                new DummyInterceptor(),
                new DummyInterceptor()
            };
            
            interceptors.AddRange(interceptorList);
            Assert.Equal(2, interceptors.Count);
            
            var interceptors2 = new RequestInterceptors();
            interceptors2.Add<DummyInterceptor>();
            interceptors2.AddRange(interceptors);
            Assert.Equal(3, interceptors2.Count);
        }

        [Fact]
        public void Intercept()
        {
            
        }
    }
}