using System;
using System.Net;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IConfig : IWebRequestConfig
    {
        int QueueWatchDelayMilliseconds { get; }
        bool RequestRecording { get; }
    }
}