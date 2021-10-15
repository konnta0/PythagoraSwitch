using System;
using System.Collections.Generic;
using konnta0.Exceptions;
using PythagoraSwitch.Recorder.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public sealed class Recorder : IRecorder
    {
        private readonly List<IRequestRecordContent> _recordContents = new List<IRequestRecordContent>();
        private bool _recording;
        private readonly IWebRequestExporter _exporter;
        private DateTime _startAt;

        public Recorder(IWebRequestExporter exporter)
        {
            _exporter = exporter;
        }

        public IErrors Start()
        {
            if (_recording)
            {
                return Errors.New("already staring. please call Stop()");
            }

            _recording = true;
            _startAt = DateTime.Now;
            return Errors.Nothing();
        }

        public void Add(IRequestRecordContent content)
        {
            if (!_recording)
            {
                return;
            }
            _recordContents.Add(content);
        }

        public IErrors Stop()
        {
            if (!_recording)
            {
                return Errors.New("already stopped. please call Start()");
            }

            _recording = false;
            return Errors.Nothing();
        }

        public IErrors Clear()
        {
            _recordContents.Clear();
            return Errors.Nothing();
        }

        public (string, IErrors) Export()
        {
            return _exporter.Handle(_recordContents);
        }
        
    }
}
