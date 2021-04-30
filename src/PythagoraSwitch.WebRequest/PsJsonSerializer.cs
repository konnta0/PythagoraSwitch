using System.Text.Json;
using konnta0.Exceptions;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public sealed class PsJsonSerializer : IPsSerializer
    {
        public string ContentType => "application/json";

        public (string, IErrors) Serialize<TReq>(TReq req) where TReq : IPsWebRequestContent
        {
            var str = string.Empty;
            var error = Errors.Try(() =>
            {
                str = JsonSerializer.Serialize(req);
            });
            return (str, error);
        }

        public (TRes, IErrors) Deserialize<TRes>(string message) where TRes : IPsWebResponseContent
        {
            TRes responseContent = default;
            var error = Errors.Try(() =>
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }; 
                responseContent = JsonSerializer.Deserialize<TRes>(message, options);
            });
            return (responseContent, error);
        }
    }
}