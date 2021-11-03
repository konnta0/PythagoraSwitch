using System;
using System.Collections.Generic;
using PythagoraSwitch.Recorder;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.Test.Recorder;

namespace PythagoraSwitch.Test
{
    internal static class DummyRecordFactory
    {
        public static List<IRequestRecordContent> CreateByInterface()
        {
            return new List<IRequestRecordContent>(Create());;
        }
        
        public static List<RequestRecordContent> Create()
        {
            return new List<RequestRecordContent>
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