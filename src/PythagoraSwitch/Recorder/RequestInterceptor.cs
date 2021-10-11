using System;
using konnta0.Exceptions;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public class RequestInterceptor : IPsRequestInterceptor
    {
        private readonly IPsRecorder _recorder;

        public Func<IPsWebRequestContent, (IPsWebResponseContent, IErrors)> NextFunc { get; set; }
        
        public (IPsWebResponseContent, IErrors) Handle(IPsWebRequestContent content)
        {
            return NextFunc(content);
        }
    }
}