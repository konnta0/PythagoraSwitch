using System;
using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public sealed class RequestInterceptor : IWebRequestInterceptor
    {
        private readonly IRecorder _recorder;

        public RequestInterceptor(IRecorder recorder)
        {
            _recorder = recorder;
            _recorder.Start();
        }

        public async Task<(TRes, IErrors)> Handle<TRes>(RequestInfo requestInfo, Func<RequestInfo, Task<(TRes, IErrors)>> next) where TRes : IWebResponseContent
        {
            _recorder.Add(new RequestRecordContent
            {
                Method = requestInfo.Method.ToString(),
                EndPoint = requestInfo.Uri.ToString(),
                RequestContent = requestInfo.Content,
                RequestContentType = requestInfo.ContentType,
                Headers = requestInfo.Headers
            });
            return await next(requestInfo);
        }
    }
}