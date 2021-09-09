using System;
using System.Collections.Generic;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public sealed class PsRequestRecordContent : IPsRequestRecordContent
    {
        public TimeSpan Interval { get; set; }

        public string EndPoint { get; set; }

        public string Method { get; set; }

        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; set; }

        public IPsWebRequestContent RequestContent { get; set; }

        private DateTime _requestStartAt;

        public void RequestStart()
        {
            _requestStartAt = DateTime.Now;
        }

        public DateTime GetRequestStartAt() => _requestStartAt;
    }
}
