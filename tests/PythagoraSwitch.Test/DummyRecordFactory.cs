using System;
using System.Collections.Generic;
using PythagoraSwitch.Recorder;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.Test.Recorder;

namespace PythagoraSwitch.Test
{
    internal static class DummyRecordFactory
    {
        public static List<IPsRequestRecordContent> Create()
        {
            return new List<IPsRequestRecordContent>
            {
                PsRequestRecordContent.Create(
                    new TimeSpan(0),
                    "api/dummy/path",
                    "GET",
                    new DummyWebContent.DummyRequestContent
                    {
                        Id = 123,
                        innner = new DummyWebContent.InnerObject
                        {
                            InnerMessage = "inner_message"
                        }
                    }
                ),
                PsRequestRecordContent.Create(
                    new TimeSpan(20),
                    "api/dummy/path",
                    "GET",
                    new DummyWebContent.DummyRequestContent()
                )
            };
        }
    }
}