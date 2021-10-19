using System;
using System.Collections.Generic;
using System.Net;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IWebRequestConfig
    {
        TimeSpan Timeout { get; }
        int RetryCount { get; }
        HttpStatusCode[] RetryHttpStatusCodes { get; }
        Func<int, TimeSpan> RetrySleepDurationProvider { get; }
        List<KeyValuePair<string, List<string>>> Headers { get; }
    }
}