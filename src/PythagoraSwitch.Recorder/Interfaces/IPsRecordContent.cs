using System;
namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsRecordContent
    {
        TimeSpan Start { get; init; }
        TimeSpan End { get; init; }
        string EndPoint { get; init; }
        string Method { get; init; }
        PythagoraSwitch.WebRequest.Interfaces.IPsWebRequestContent RequestContent { get; init; }
        PythagoraSwitch.WebRequest.Interfaces.IPsWebResponseContent ResponseContent { get; init; }
    }
}
