using System;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IPsRequestQueue
    {
        void Enqueue(IPsRequest request);
        IPsRequest Dequeue();
        void WatchRequestQueue(int queueWatchDelayMilliseconds, Action<IPsRequest> requestCallback);
    }
}