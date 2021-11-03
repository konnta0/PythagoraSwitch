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

        public IWebRequestConfig Config { get; set; }

        public IWebRequestContent Content { get; set; }
        
        public Type ContentType { get; set; }
    }
}