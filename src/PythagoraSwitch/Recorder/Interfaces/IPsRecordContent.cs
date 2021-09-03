using System;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsRecordContent
    {
        TimeSpan Start { get; set; }
        TimeSpan End { get; set; }
        string EndPoint { get; set; }
        string Method { get; set; }
        
        PythagoraSwitch.WebRequest.Interfaces.IPsWebRequestContent RequestContent { get; set; }
        PythagoraSwitch.WebRequest.Interfaces.IPsWebResponseContent ResponseContent { get; set; }
    }
}
