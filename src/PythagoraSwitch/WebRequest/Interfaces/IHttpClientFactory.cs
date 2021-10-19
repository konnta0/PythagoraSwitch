using System.Net.Http;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IHttpClientFactory
    {
        HttpClient Create();
        HttpClient Create(HttpClientHandler clientHandler);
        HttpClient Create(HttpMessageHandler messageHandler);
    }
}