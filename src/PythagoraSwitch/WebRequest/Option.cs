using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public class Option : IOption
    {
        public RequestInterceptors RequestInterceptors { get; } = new RequestInterceptors();
    }
}