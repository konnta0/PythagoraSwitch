using System.Collections.Generic;
using konnta0.Exceptions;
using PythagoraSwitch.WebRequest.Recorder.Interfaces;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsExporter
    {
        (string, IErrors) Handle(IList<IPsRecordContent> contents);
    }
}