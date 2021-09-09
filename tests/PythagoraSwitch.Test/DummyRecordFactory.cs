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
                new PsRequestRecordContent
                {
                    Interval = new TimeSpan(0),
                    EndPoint = "api/dummy/path",
                    Method = "GET",
                    RequestContent = new DummyWebContent.DummyRequestContent
                    {
                        Id = 123,
                        innner = new DummyWebContent.InnerObject
                        {
                            InnerMessage = "inner_message"
                        }
                    }
                }
                ,
                new PsRequestRecordContent{
                    Interval = new TimeSpan(20),
                    EndPoint = "api/dummy/path",
                    Method = "GET",
                    RequestContent = new DummyWebContent.DummyRequestContent()
                }
            };
        }
    }
}