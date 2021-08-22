using System;
using System.Collections.Generic;
using System.Net;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public sealed class PsDefaultConfig : IPsConfig
    {
        public TimeSpan Timeout => new TimeSpan(0, 0, 30);
        public int RetryCount => 3;
        public HttpStatusCode[] RetryHttpStatusCodes => new HttpStatusCode[]
        {
            (HttpStatusCode)429,
            HttpStatusCode.ServiceUnavailable
        };

        public Func<int, TimeSpan> RetrySleepDurationProvider =>
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));

        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers =>
            new List<KeyValuePair<string, IEnumerable<string>>>();
        public int QueueWatchDelayMilliseconds => 20;
    }
}