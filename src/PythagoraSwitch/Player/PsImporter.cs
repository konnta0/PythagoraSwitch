using System.Collections.Generic;
using System.IO;
using konnta0.Exceptions;
using Microsoft.Extensions.Logging;
using PythagoraSwitch.Player.Interfaces;
using PythagoraSwitch.WebRequest.Recorder.Interfaces;
using YamlDotNet.Serialization;

namespace PythagoraSwitch.Player
{
    public class PsImporter : IPsImporter
    {
        private readonly IPsImporterConfig _config;
        private readonly ILogger<PsImporter> _logger;

        public PsImporter(IPsImporterConfig config, ILogger<PsImporter> logger)
        {
            _config = config;
            _logger = logger;
        }
        
        public (IList<IPsRecordContent>, IErrors) Handle(string path)
        {
            if (!File.Exists(path))
            {
                return (default, Errors.New<FileNotFoundException>());
            }

            IList<IPsRecordContent> records = default;
            var error = Errors.Try(delegate
            {
                var deserializer = new Deserializer();
                records = deserializer.Deserialize<IList<IPsRecordContent>>(path);
            });
            return (records, error);
        }
    }
}