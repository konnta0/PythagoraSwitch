using System;
using System.Collections.Generic;
using PythagoraSwitch.Recorder;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.Test.Recorder;

namespace PythagoraSwitch.Test
{
    internal static class DummyRecordFactory
    {
        public static List<IPsRequestRecordContent> CreateByInterface()
        {
            return new List<IPsRequestRecordContent>(Create());;
        }
        
        public static List<PsRequestRecordContent> Create()
        {
            return new List<PsRequestRecordContent>
            {
                new()
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
                    },
                    RequestContentType = typeof(DummyWebContent.DummyRequestContent)
                }
                ,
                new()
                {
                    Interval = new TimeSpan(20),
                    EndPoint = "api/dummy/path",
                    Method = "GET",
                    RequestContent = new DummyWebContent.DummyRequestContent(),
                    RequestContentType = typeof(DummyWebContent.DummyRequestContent)
                }
            };
        }
    }
}