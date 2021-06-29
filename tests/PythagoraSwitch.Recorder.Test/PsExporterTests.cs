using System;
using System.Collections.Generic;
using PythagoraSwitch.Recorder.Interfaces;
using Xunit;

namespace PythagoraSwitch.Recorder.Test
{
    public class PsExporterTests
    {
        [Fact]
        internal void HandleTest()
        {
            var exporter = new PsExporter(new DefaultExporterConfig(), LoggerFactory.Create<PsExporter>());

            var recordContents = new List<IPsRecordContent>
            {
                PsRecordContent.Create(
                    new TimeSpan(0),
                    new TimeSpan(1),
                    new DummyWebContent.DummyRequestContent
                    {
                        Id = 123,
                        innner = new DummyWebContent.InnerObject
                        {
                            InnerMessage = "inner_message"
                        }
                    },
                    new DummyWebContent.DummyResponseContent
                    {
                        InnerObjects = new List<DummyWebContent.InnerObject>
                        {
                            new (){InnerMessage = "list_item1"},
                            new (){InnerMessage = "list_item2"}
                        }
                    }
                ),
                PsRecordContent.Create(
                    new TimeSpan(2),
                    new TimeSpan(3),
                    new DummyWebContent.DummyRequestContent(),
                    new DummyWebContent.DummyResponseContent()
                )
            };
            var errors = exporter.Handle(recordContents);
            Assert.Null(errors);
        }
    }
}