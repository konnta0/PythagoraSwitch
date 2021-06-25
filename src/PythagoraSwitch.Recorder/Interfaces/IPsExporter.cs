using System.Collections.Generic;
using konnta0.Exceptions;

namespace PythagoraSwitch.Recorder.Interfaces
{
    public interface IPsExporter
    {
        IErrors Handle(IList<IPsRecordContent> contents);
    }
}