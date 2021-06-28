using System;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public class PsRecordContent : IPsRecordContent
    {
        public TimeSpan Start { get; init; }
        public TimeSpan End { get; init; }
        public IPsWebRequestContent RequestContent { get; init; }
        public IPsWebResponseContent ResponseContent { get; init; }

        public static PsRecordContent Create(TimeSpan start, TimeSpan end, IPsWebRequestContent requestContent, IPsWebResponseContent responseContent)
        {
            return new ()
                {Start = start, End = end, RequestContent = requestContent, ResponseContent = responseContent};
        }
    }
}
