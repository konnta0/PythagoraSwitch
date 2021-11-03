namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IConfig : IWebRequestConfig
    {
        int QueueWatchDelayMilliseconds { get; }
    }
}