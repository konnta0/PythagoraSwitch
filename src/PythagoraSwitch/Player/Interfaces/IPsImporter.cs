using System.Collections.Generic;
using konnta0.Exceptions;
using PythagoraSwitch.Recorder.Interfaces;

namespace PythagoraSwitch.Player.Interfaces
{
    public interface IPsImporter
    {
        (IList<T>, IErrors) Handle<T>(string path) where T : IPsRequestRecordContent;
    }
}