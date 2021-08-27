using System.Collections.Generic;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.Test.Recorder
{
    internal class DummyWebContent
    {
        internal class DummyRequestContent : IPsWebRequestContent
        {
            public int Id { get; init; }
            public InnerObject innner { get; init; }
        }
        

        internal class DummyResponseContent : IPsWebResponseContent
        {
            public string Message { get; init; }
            public List<InnerObject> InnerObjects { get; init; }
        }

        internal class InnerObject
        {
            public string InnerMessage { get; init; }
        }
    }
}