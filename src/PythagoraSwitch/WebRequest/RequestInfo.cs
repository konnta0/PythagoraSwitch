using System;
using System.Collections.Generic;
using System.Net.Http;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public class RequestInfo
    {
        public Uri Uri { get; set; }

        public HttpMethod Method { get; set; }

        public List<KeyValuePair<string, List<string>>> Headers { get; set; }

        public IPsWebRequestConfig Config { get; set; }

        public IPsWebRequestContent Content { get; set; }
        
        public Type ContentType { get; set; }
    }
}