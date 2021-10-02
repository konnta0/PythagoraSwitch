using System.Collections.Generic;
using System.IO;
using konnta0.Exceptions;
using Microsoft.Extensions.Logging;
using PythagoraSwitch.Player.Interfaces;
using PythagoraSwitch.Recorder.Interfaces;
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
        
        public (IList<T>, IErrors) Handle<T>(string path) where T : IPsRequestRecordContent
        {
            if (!File.Exists(path))
            {
                return (default, Errors.New<FileNotFoundException>());
            }

            IList<T> records = default;
            var error = Errors.Try(delegate
            {
                var deserializer = new Deserializer();
                var yml = File.ReadAllText(path);
                records = deserializer.Deserialize<List<T>>(yml);
            });
            return (records, error);
        }
    }
}