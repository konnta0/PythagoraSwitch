using System;
using System.Net;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IPsConfig : IPsWebRequestConfig
    {
        int QueueWatchDelayMilliseconds { get; }
        bool RequestRecording { get; }
    }
}