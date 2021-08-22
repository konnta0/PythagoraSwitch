using System;
using PythagoraSwitch.WebRequest.Interfaces;
using PythagoraSwitch.WebRequest.Recorder.Interfaces;

namespace PythagoraSwitch.WebRequest.Recorder
{
    public class PsRecordContent : IPsRecordContent
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string EndPoint { get; set; }
        public string Method { get; set; }
        public IPsWebRequestContent RequestContent { get; set; }
        public IPsWebResponseContent ResponseContent { get; set; }

        public static PsRecordContent Create(TimeSpan start, TimeSpan end, string endPoint, string method, IPsWebRequestContent requestContent, IPsWebResponseContent responseContent)
        {
            return new PsRecordContent
                {Start = start, End = end, EndPoint = endPoint, Method = method, RequestContent = requestContent, ResponseContent = responseContent};
        }
    }
}
