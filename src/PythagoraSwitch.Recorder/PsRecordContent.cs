using System;
using PythagoraSwitch.Recorder.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public class PsRecordContent : IPsRecordContent
    {
        public TimeSpan start { get; init; }
        public TimeSpan end { get; init; }
        public PythagoraSwitch.WebRequest.Interfaces.IPsWebRequestContent requestContent { get; init; }
        public PythagoraSwitch.WebRequest.Interfaces.IPsWebResponseContent responseContent { get; init; }
    }
}
