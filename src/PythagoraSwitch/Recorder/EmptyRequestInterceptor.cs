using System;
using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public sealed class EmptyRequestInterceptor : IWebRequestRecorderInterceptor
    {
        public async Task<(TRes, IErrors)> Handle<TRes>(RequestInfo requestInfo, Func<RequestInfo, Task<(TRes, IErrors)>> next) where TRes : IWebResponseContent
        {
            return await next(requestInfo);
        }
    }
}