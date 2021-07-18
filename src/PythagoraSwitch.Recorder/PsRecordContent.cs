using System;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public class PsRecordContent : IPsRecordContent
    {
        public TimeSpan Start { get; init; }
        public TimeSpan End { get; init; }
        public string EndPoint { get; init; }
        public string Method { get; init; }
        public IPsWebRequestContent RequestContent { get; init; }
        public IPsWebResponseContent ResponseContent { get; init; }

        public static PsRecordContent Create(TimeSpan start, TimeSpan end, string endPoint, string method, IPsWebRequestContent requestContent, IPsWebResponseContent responseContent)
        {
            return new ()
                {Start = start, End = end, EndPoint = endPoint, Method = method, RequestContent = requestContent, ResponseContent = responseContent};
        }
    }
}
