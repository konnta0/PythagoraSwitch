using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.Recorder
{
    public abstract class ScenarioAbstract : IScenario
    {
        protected IList<IRequestRecordContent> Records;
        protected int Index;
        protected IRequestRecordContent CurrentRecord;
        protected IWebRequestHandler _webRequestHandler;
        protected IList<Func<Uri, IOption, Task<(string, IErrors)>>> Order;

        protected ScenarioAbstract(IWebRequestHandler webRequestHandler)
        {
            _webRequestHandler = webRequestHandler;
            Reset();
            Records = new List<IRequestRecordContent>();
            Order = new List<Func<Uri, IOption, Task<(string, IErrors)>>>();
        }

        public bool MoveNext()
        {
            if (++Index >= Records.Count) return false;
            CurrentRecord = Records[Index];
            return true;
        }

        public void Reset()
        {
            CurrentRecord = null;
            Index = 0;
        }

        public IRequestRecordContent Current => CurrentRecord;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Reset();
        }
    }
}