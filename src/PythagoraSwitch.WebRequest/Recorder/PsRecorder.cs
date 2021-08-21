using System.Collections.Generic;
using konnta0.Exceptions;
using PythagoraSwitch.WebRequest.Recorder.Interfaces;

namespace PythagoraSwitch.WebRequest.Recorder
{
    public class PsRecorder : IPsRecorder
    {
        private readonly List<IPsRecordContent> _recordContents = new List<IPsRecordContent>();
        private bool _recording;
        private readonly IPsExporter _exporter;

        public PsRecorder(IPsExporter exporter)
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
            return Errors.Nothing();
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

        public IErrors Export()
        {
            return _exporter.Handle(_recordContents);
        }
        
    }
}
