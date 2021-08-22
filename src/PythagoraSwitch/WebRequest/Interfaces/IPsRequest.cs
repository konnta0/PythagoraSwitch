using System;
using System.Threading.Tasks;
using konnta0.Exceptions;

namespace PythagoraSwitch.WebRequest.Interfaces
{
    public interface IPsRequest
    {
        Task<(string, IErrors)> HandleTask { get; set; }
        Action<(string, IErrors)> OnResponse { get; set; }
    }
}