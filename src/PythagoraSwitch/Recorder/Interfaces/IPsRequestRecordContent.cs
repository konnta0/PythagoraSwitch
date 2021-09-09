using System;
using System.Collections.Generic;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsRequestRecordContent
    {
        TimeSpan Interval { get; set; }

        string EndPoint { get; set; }

        string Method { get; set; }

        IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; set; }

        PythagoraSwitch.WebRequest.Interfaces.IPsWebRequestContent RequestContent { get; set; }
    }
}
