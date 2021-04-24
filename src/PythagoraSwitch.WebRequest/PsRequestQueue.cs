using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public class Request : IPsRequest
    {
        public Task<(string, IErrors)> HandleTask { get; set; }
        public Action<(string, IErrors)> OnResponse { get; set; }
    }

    public class PsRequestQueue : IPsRequestQueue
    {
        private readonly Queue<IPsRequest> _requestQueue;

        public PsRequestQueue()
        {
            _requestQueue = new Queue<IPsRequest>();
        }

        public void Enqueue(IPsRequest request)
        {
            _requestQueue.Enqueue(request);
        }

        public IPsRequest Dequeue()
        {
            return _requestQueue.Dequeue();
        }

        public async void WatchRequestQueue(int queueWatchDelayMilliseconds, Action<IPsRequest> requestCallback)
        {
            while (true)
            {
                if (_requestQueue.Count == 0)
                {
                    await Task.Delay(queueWatchDelayMilliseconds);
                    continue;
                }

                requestCallback(_requestQueue.Dequeue());
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}