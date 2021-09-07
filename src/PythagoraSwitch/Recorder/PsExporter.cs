using System;
using System.Collections.Generic;
using System.IO;
using konnta0.Exceptions;
using Microsoft.Extensions.Logging;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest.Recorder.Interfaces;
using YamlDotNet.Serialization;

namespace PythagoraSwitch.Recorder
{
    public sealed class PsExporter : IPsExporter
    {
        private readonly IPsExporterConfig _config;
        private readonly ILogger<PsExporter> _logger;

        public PsExporter(IPsExporterConfig config, ILogger<PsExporter> logger)
        {
            _config = config;
            _logger = logger;
        }

        public (string, IErrors) Handle(IList<IPsRequestRecordContent> contents)
        {
            var (path, error) = Errors.Try<string>(delegate
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
                return filePath;
            });

            if (Errors.IsOccurred(error))
            {
                _logger.LogError($"Export failed. reason: {error.Exception.Message}");
            }
            return (path, error);
        }

        private static (string, IErrors) Serialize(IList<IPsRequestRecordContent> contents)
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