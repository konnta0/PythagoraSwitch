using System;
using PythagoraSwitch.Recorder.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public class PsRecordContent : IPsRecordContent
    {
        public TimeSpan Start { get; init; }
        public TimeSpan End { get; init; }
        public PythagoraSwitch.WebRequest.Interfaces.IPsWebRequestContent RequestContent { get; init; }
        public PythagoraSwitch.WebRequest.Interfaces.IPsWebResponseContent ResponseContent { get; init; }
    }
}
