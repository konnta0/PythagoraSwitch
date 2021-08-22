using System.Dynamic;
using System.Net.Http;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IPsHttpClientFactory
    {
        HttpClient Create();
        HttpClient Create(HttpClientHandler clientHandler);
        HttpClient Create(HttpMessageHandler messageHandler);
    }
}