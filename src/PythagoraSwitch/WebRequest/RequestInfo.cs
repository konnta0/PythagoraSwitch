using System;
using System.Collections.Generic;
using System.Net.Http;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public class RequestInfo
    {
        Uri Uri { get; set; }

        HttpMethod Method { get; set; }

        List<KeyValuePair<string, List<string>>> Headers { get; set; }

        IPsWebRequestContent RequestContent { get; set; }
    }
}