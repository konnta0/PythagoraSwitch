using System.Collections.Generic;
using konnta0.Exceptions;
using PythagoraSwitch.WebRequest.Recorder.Interfaces;

namespace PythagoraSwitch.Player.Interfaces
{
    public interface IPsImporter
    {
        (IList<IPsRecordContent>, IErrors) Handle(string path);
    }
}