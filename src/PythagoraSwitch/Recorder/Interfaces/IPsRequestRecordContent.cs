using System;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsRequestRecordContent
    {
        TimeSpan Interval { get; set; }
        string EndPoint { get; set; }
        string Method { get; set; }
        
        PythagoraSwitch.WebRequest.Interfaces.IPsWebRequestContent RequestContent { get; set; }
    }
}
