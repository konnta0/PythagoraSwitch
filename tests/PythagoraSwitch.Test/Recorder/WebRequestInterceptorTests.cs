using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using konnta0.Exceptions;
using Moq;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest;
using Xunit;
using WebRequestInterceptor = PythagoraSwitch.Recorder.WebRequestInterceptor;

namespace PythagoraSwitch.Test.Recorder
{
    public class WebRequestInterceptorTests
    {
        [Fact]
        private async void HandleTest()
        {
            var recorderMock = new Mock<IRecorder>();
            recorderMock.Setup(m => m.Start()).Returns(Errors.Nothing);
            recorderMock.Setup(m => m.Add(It.IsAny<IRequestRecordContent>()));

            var interceptor = new WebRequestInterceptor(recorderMock.Object);
            var (response, errors) = await interceptor.Handle(new RequestInfo
            {
                Uri = new Uri("https://hoge.com/dummy/path"),
                Config = new WebRequest.WebRequestInterceptorTests.Config(),
                Content = new DummyWebContent.DummyRequestContent(),
                Headers = new List<KeyValuePair<string, List<string>>>{new("dummy-header", new List<string>{"dummy-value"})},
                ContentType = typeof(DummyWebContent.DummyRequestContent),
                Method = HttpMethod.Post
            }, EmptyNext);
            Assert.Equal(Errors.Nothing(), errors);
            Assert.NotNull(response);
        }

        private async Task<(DummyWebContent.DummyResponseContent, IErrors)> EmptyNext(RequestInfo _)
        {
            return (new DummyWebContent.DummyResponseContent(), Errors.Nothing());
        }
    }
}