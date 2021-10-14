using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public sealed class PsRequestQueue : IPsRequestQueue
    {
        private readonly Queue<Task> _requestQueue;

        public PsRequestQueue()
        {
            _requestQueue = new Queue<Task>();
        }

        public void Enqueue(Task requestTask)
        {
            _requestQueue.Enqueue(requestTask);
        }

        public Task Dequeue()
        {
            return _requestQueue.Dequeue();
        }

        public async void WatchRequestQueue(int queueWatchDelayMilliseconds, Action<Task> requestCallback, CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested) break;

                if (_requestQueue.Count == 0)
                {
                    await Task.Delay(queueWatchDelayMilliseconds);
                    continue;
                }

                requestCallback(_requestQueue.Dequeue());
            }
        }
    }
}