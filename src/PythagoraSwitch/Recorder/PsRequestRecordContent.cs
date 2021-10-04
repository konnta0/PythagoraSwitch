using System;
using System.Collections.Generic;
using PythagoraSwitch.Recorder.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public sealed class PsRequestRecordContent : IPsRequestRecordContent
    {
        public TimeSpan Interval { get; set; }

        public string EndPoint { get; set; }

        public string Method { get; set; }

        public List<KeyValuePair<string, List<string>>> Headers { get; set; }

        public object RequestContent { get; set; }

        public Type RequestContentType { get; set; }

        private DateTime _requestStartAt;

        public void RequestStart()
        {
            _requestStartAt = DateTime.Now;
        }

        public DateTime GetRequestStartAt() => _requestStartAt;
    }
}
