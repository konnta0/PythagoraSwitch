using System.Collections.Generic;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.Test.Recorder
{
    internal class DummyWebContent
    {
        internal class DummyRequestContent : IWebRequestContent
        {
            public int Id { get; set; }
            public InnerObject innner { get; set; }
        }
        

        internal class DummyResponseContent : IWebResponseContent
        {
            public string Message { get; set; }
            public List<InnerObject> InnerObjects { get; set; }
        }

        internal class InnerObject
        {
            public string InnerMessage { get; set; }
        }
    }
}