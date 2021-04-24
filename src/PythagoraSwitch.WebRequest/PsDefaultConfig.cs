using System;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public class PsDefaultConfig : IPsConfig
    {
        public TimeSpan Timeout => new TimeSpan(0, 0, 30);
        public int QueueWatchDelayMilliseconds => 20;
    }
}