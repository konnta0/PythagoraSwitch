using System.Net.Http;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public sealed class PsHttpClientFactory : IPsHttpClientFactory
    {
        public HttpClient Create()
        {
            return new HttpClient();
        }

        public HttpClient Create(HttpClientHandler clientHandler)
        {
            return new HttpClient(clientHandler);
        }

        public HttpClient Create(HttpMessageHandler messageHandler)
        {
            return new HttpClient(messageHandler);
        }
    }
}