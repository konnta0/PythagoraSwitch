using System.Collections.Generic;
using konnta0.Exceptions;
using PythagoraSwitch.Player.Interfaces;
using PythagoraSwitch.WebRequest.Recorder.Interfaces;

namespace PythagoraSwitch.Player
{
    public class PsImporter : IPsImporter
    {
        private readonly IPsImporterConfig _config;

        public PsImporter(IPsImporterConfig config)
        {
            _config = config;
        }
        
        public (IList<IPsRecordContent>, IErrors) Handle(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}