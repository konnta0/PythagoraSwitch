using System.Collections.Generic;
using konnta0.Exceptions;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IWebRequestExporter
    {
        (string, IErrors) Handle(string scenarioName, IList<IRequestRecordContent> contents);
        (string, IErrors) Handle(IList<IRequestRecordContent> contents);
    }
}