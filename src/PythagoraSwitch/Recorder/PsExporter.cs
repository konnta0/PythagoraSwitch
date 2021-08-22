using System;
using System.Collections.Generic;
using System.IO;
using konnta0.Exceptions;
using Microsoft.Extensions.Logging;
using PythagoraSwitch.WebRequest.Recorder.Interfaces;
using YamlDotNet.Serialization;

namespace PythagoraSwitch.WebRequest.Recorder
{
    public class PsExporter : IPsExporter
    {
        private readonly IPsExporterConfig _config;
        private readonly ILogger<PsExporter> _logger;

        public PsExporter(IPsExporterConfig config, ILogger<PsExporter> logger)
        {
            _config = config;
            _logger = logger;
        }

        public IErrors Handle(IList<IPsRecordContent> contents)
        {
            var error = Errors.Try(delegate
            {
                var (text, serializeError) = Serialize(contents);
                if (Errors.IsOccurred(serializeError))
                {
                    throw serializeError.Exception;
                }

                var filePath = Path.Combine(_config.OutPath,
                    $"{_config.FilePrefix}-{DateTime.Now:yyyyMMddHHmmss}.yaml");
                using var writer = File.CreateText(filePath);
                writer.Write(text);
            });

            if (Errors.IsOccurred(error))
            {
                _logger.LogError($"Export failed. reason: {error.Exception.Message}");
            }
            return error;
        }

        private static (string, IErrors) Serialize(IList<IPsRecordContent> contents)
        {
            var serializedString = string.Empty;
            var errors = Errors.Try(delegate
            {
                var serializer = new Serializer();
                serializedString = serializer.Serialize(contents);       
            });
            return (serializedString, errors);
        }
    }
}