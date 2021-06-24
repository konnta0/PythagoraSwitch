using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.Recorder.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public class PsRecorder : IPsRecorder
    {
        private List<PythagoraSwitch.WebRequest.Interfaces.IPsWebRequestContent> _requestContents = new();
        private bool _isRecording = false;

        public PsRecorder()
        {
        }

        public IErrors Clear()
        {
            _requestContents.Clear();
            return Errors.Nothing();
        }

        public IErrors Export(string outPath)
        {
            throw new NotImplementedException();
        }

        public IErrors Start()
        {
            if (_isRecording)
            {
                return Errors.New("already staring. please call Stop()");
            }

            _isRecording = true;
            return Errors.Nothing();
        }

        public IErrors Stop()
        {
            if (!_isRecording)
            {
                return Errors.New("already stopped. please call Start()");
            }

            _isRecording = false;
            return Errors.Nothing();
        }
    }
}
