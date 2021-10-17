using System;
using System.Threading;
using System.Threading.Tasks;
using PythagoraSwitch.WebRequest;
using Xunit;

namespace PythagoraSwitch.Test.WebRequest
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
        public async void WatchRequestQueueTest()
        {
            var tokenSource = new CancellationTokenSource();
            var queue = new RequestQueue();
            var request1 = Task.Delay(100);

            var called = 0;
            queue.WatchRequestQueue(1, delegate(Task t)
            {
                if (t == null) throw new ArgumentNullException(nameof(t));
                Assert.Same(request1, t);
                called++;
            },tokenSource.Token);

            queue.Enqueue(request1);
            await Task.Delay(10);

            tokenSource.Cancel();
            Assert.Equal(1, called);
        }
    }
}