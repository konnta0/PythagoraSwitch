using System;
using System.Collections.Generic;
using System.IO;
using konnta0.Exceptions;
using Microsoft.Extensions.Logging;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest.Interfaces;
using Scriban;
using Scriban.Runtime;
using YamlDotNet.Serialization;

namespace PythagoraSwitch.Recorder
{
    public sealed class WebRequestExporter : IWebRequestExporter
    {
        private readonly IExporterConfig _config;
        private readonly IScenarioTemplate _scenarioTemplate;
        private readonly ILogger<WebRequestExporter> _logger;

        public WebRequestExporter(IExporterConfig config, IScenarioTemplate scenarioTemplate, ILogger<WebRequestExporter> logger)
        {
            _config = config;
            _scenarioTemplate = scenarioTemplate;
            _logger = logger;
        }

        public (string, IErrors) Handle(string scenarioName, IList<IRequestRecordContent> contents)
        {
            if (string.IsNullOrEmpty(scenarioName))
            {
                return (string.Empty, Errors.New("scenarioName must not be null or empty"));
            }
            
            var (generatedCsText, generateError) = Errors.Try(delegate
            {
                var scriptObject = new ScriptObject();
                scriptObject.Import(typeof(WebRequestExporterExtension));
                scriptObject.Add("scenario_name", scenarioName);
                scriptObject.Add("contents", contents);
                
                var context = new TemplateContext();
                context.PushGlobal(scriptObject);

                var template = Template.Parse(_scenarioTemplate.GetSyntax);
                var text = template.Render(context);
                
                return text;
            });

            if (Errors.IsOccurred(generateError))
            {
                _logger.LogError("Failed generate scenario. ScenarioName: {ScenarioName}", scenarioName);
                return (string.Empty, generateError);
            }
            
            var (wroteFilePath, errors) = Errors.Try(delegate
            {
                var filePath = $"{scenarioName}.Generated.cs";
                using var writer = File.CreateText(filePath);
                writer.Write(generatedCsText);
                return filePath;
            });
            
            if (Errors.IsOccurred(errors))
            {
                _logger.LogError("Export failed. reason: {Reason}", errors.Exception.Message);
            }

            return (wroteFilePath, errors);
        }

        public (string, IErrors) Handle(IList<IRequestRecordContent> contents)
        {
            var (path, error) = Errors.Try(delegate
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

        private static (string, IErrors) Serialize(IList<IRequestRecordContent> contents)
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


    public static class WebRequestExporterExtension
    {
        public static string RequestContentTypeToString(Type type)
        {
            return type.Name;
        }

        public static string ContentImplementInterfaceString(string method)
        {
            if (method == "GET")
            {
                return nameof(IWebGetRequestContent);
            }

            if (method == "POST")
            {
                return nameof(IWebPostRequestContent);
            }

            throw new ArgumentException();
        }
    }
}