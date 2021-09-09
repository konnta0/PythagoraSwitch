using System;
using System.Collections.Generic;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsRequestRecordContent
    {
        TimeSpan Interval { get; set; }

        string EndPoint { get; set; }

        string Method { get; set; }

        List<KeyValuePair<string, List<string>>> Headers { get; set; }

        object RequestContent { get; set; }

        Type RequestContentType { get; set; }
    }
}
