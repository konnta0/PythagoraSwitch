using System;
using System.Threading;
using System.Threading.Tasks;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IRequestQueue
    {
        void Enqueue(Task requestTask);
        Task Dequeue();
        void WatchRequestQueue(int queueWatchDelayMilliseconds, Action<Task> requestCallback, CancellationToken token);
    }
}