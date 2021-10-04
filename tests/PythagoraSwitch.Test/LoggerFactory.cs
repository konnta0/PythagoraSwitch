using Microsoft.Extensions.Logging;
using Moq;

namespace PythagoraSwitch.Test
{
    internal static class LoggerFactory
    {
        internal static ILogger<T> Create<T>()
        {
            var mock = new Mock<ILogger<T>>();
            return mock.Object;
        }
    }
}