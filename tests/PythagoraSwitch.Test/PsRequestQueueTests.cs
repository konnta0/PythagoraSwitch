using System;
using System.Threading;
using System.Threading.Tasks;
using konnta0.Exceptions;
using PythagoraSwitch.WebRequest;
using PythagoraSwitch.WebRequest.Interfaces;
using Xunit;

namespace PythagoraSwitch.Test
{
    public class PsRequestQueueTests
    {
        [Fact]
        public void EnqueueDequeueTest()
        {
            var queue = new PsRequestQueue();

            var request1 = new Request { HandleTask = new Task<(string, IErrors)>(() => (string.Empty, Errors.Nothing())), OnResponse = tuple => {}};
            var request2 = new Request { HandleTask = new Task<(string, IErrors)>(() => (string.Empty, Errors.Nothing())), OnResponse = tuple => {}};

            queue.Enqueue(request1);
            queue.Enqueue(request2);
            Assert.Same(request1, queue.Dequeue());
            Assert.Same(request2, queue.Dequeue());

            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }
        

        [Fact(Timeout = 400)]
        public async void WatchRequestQueueTest()
        {
            var tokenSource = new CancellationTokenSource();
            var queue = new PsRequestQueue();
            var request1 = new Request { HandleTask = new Task<(string, IErrors)>(() => (string.Empty, Errors.Nothing())), OnResponse = tuple => {}};

            queue.WatchRequestQueue(200, delegate(IPsRequest request)
            {
                Assert.Same(request1, request);
            },tokenSource.Token);

            queue.Enqueue(request1);
            tokenSource.Cancel();
        }
    }
}