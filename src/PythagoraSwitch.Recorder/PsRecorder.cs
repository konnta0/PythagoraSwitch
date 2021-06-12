using System;
using System.Threading.Tasks;
using PythagoraSwitch.Recorder.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public class PsRecorder : IPsRecorder
    {
        public PsRecorder()
        {
        }

        public ValueTask Clear()
        {
            throw new NotImplementedException();
        }

        public ValueTask Export()
        {
            throw new NotImplementedException();
        }

        public ValueTask Start()
        {
            throw new NotImplementedException();
        }

        public ValueTask Stop()
        {
            throw new NotImplementedException();
        }
    }
}
