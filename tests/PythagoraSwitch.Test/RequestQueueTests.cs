using System;
using System.Threading;
using System.Threading.Tasks;
using PythagoraSwitch.WebRequest;
using Xunit;

namespace PythagoraSwitch.Test
{
    public class RequestQueueTests
    {
        [Fact]
        public void EnqueueDequeueTest()
        {
            var queue = new RequestQueue();

            var request1 = Task.Run(() => {});
            var request2 = Task.Run(() => {});

            queue.Enqueue(request1);
            queue.Enqueue(request2);
            Assert.Same(request1, queue.Dequeue());
            Assert.Same(request2, queue.Dequeue());
        }
        

        [Fact(Timeout = 400)]
        public void WatchRequestQueueTest()
        {
            var tokenSource = new CancellationTokenSource();
            var queue = new RequestQueue();
            var request1 = Task.Delay(100);

            queue.WatchRequestQueue(200, delegate(Task t)
            {
                if (t == null) throw new ArgumentNullException(nameof(t));
                Assert.Same(request1, t);
            },tokenSource.Token);

            queue.Enqueue(request1);
            tokenSource.Cancel();
        }
    }
}