using System;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public class PsRequestRecordContent : IPsRequestRecordContent
    {
        public TimeSpan Interval { get; set; }
        public string EndPoint { get; set; }
        public string Method { get; set; }
        public IPsWebRequestContent RequestContent { get; set; }

        private DateTime _requestStartAt;

        public void RequestStart()
        {
            _requestStartAt = DateTime.Now;
        }

        public DateTime GetRequestStartAt() => _requestStartAt;
        
        
        public static PsRequestRecordContent Create(TimeSpan interval, string endPoint, string method, IPsWebRequestContent requestContent)
        {
            return new PsRequestRecordContent
                {Interval = interval, EndPoint = endPoint, Method = method, RequestContent = requestContent};
        }
    }
}
