using System.Collections.Generic;
using konnta0.Exceptions;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IWebRequestExporter
    {
        (string, IErrors) Handle(IList<IPsRequestRecordContent> contents);
    }
}