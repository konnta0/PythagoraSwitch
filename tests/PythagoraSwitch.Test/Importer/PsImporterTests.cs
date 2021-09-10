using System;
using System.IO;
using konnta0.Exceptions;
using PythagoraSwitch.Player;
using PythagoraSwitch.Player.Interfaces;
using PythagoraSwitch.Recorder;
using Xunit;

namespace PythagoraSwitch.Test.Importer
{
    public class PsImporterTests : IDisposable
    {
        private string _outPah;
        [Fact]
        private void Handle()
        {
            var exporterConfig = new DefaultExporterConfig();
            var exporter = new PsExporter(exporterConfig, LoggerFactory.Create<PsExporter>());
            var (outPath, _) = exporter.Handle(DummyRecordFactory.Create());
            _outPah = outPath;
            var importer = PsImporterFactory.Create();
            var (contents, error) = importer.Handle<PsRequestRecordContent>(_outPah);

            Assert.False(Errors.IsOccurred(error));
            Assert.Equal(2, contents.Count);
        }

        [Fact]
        private void HandleFileNotFoundError()
        {
            var importer = PsImporterFactory.Create();
            var (contents, error) = importer.Handle<PsRequestRecordContent>("dummy");
            Assert.True(Errors.IsOccurred(error));
            Assert.True(error.Is<FileNotFoundException>());
        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(_outPah) && File.Exists(_outPah))
            {
                File.Delete(_outPah);
            }
        }
    }

    internal static class PsImporterFactory
    {
        public static IPsImporter Create()
        {
            return new PsImporter(new PsImporterConfig(), LoggerFactory.Create<PsImporter>());
        }
    }
}