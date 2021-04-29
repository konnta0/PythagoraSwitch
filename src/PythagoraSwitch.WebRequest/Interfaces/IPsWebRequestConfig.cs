using System;
using System.Net;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IPsWebRequestConfig
    {
        TimeSpan Timeout { get; }
        int RetryCount { get; }
        HttpStatusCode[] RetryHttpStatusCodes { get; }
        Func<int, TimeSpan> RetrySleepDurationProvider { get; }
    }
}