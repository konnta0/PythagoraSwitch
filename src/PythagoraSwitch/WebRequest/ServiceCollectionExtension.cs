using System;
using Microsoft.Extensions.DependencyInjection;
using PythagoraSwitch.Recorder;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest.Interfaces;

namespace PythagoraSwitch.WebRequest
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPythagoraSwitch(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPsHttpClientFactory, PsHttpClientFactory>();
            serviceCollection.AddSingleton<IPsSerializer, PsJsonSerializer>();
            serviceCollection.AddSingleton<IRequestQueue, RequestQueue>();
            serviceCollection.AddSingleton<IWebRequestHandler, WebRequestHandler>();
            serviceCollection.AddSingleton<IRecorder, Recorder.Recorder>();
            serviceCollection.AddSingleton<IWebRequestExporter, WebRequestExporter>();
            return serviceCollection;
        }

        public static IServiceCollection AddPythagoraSwitchConfig(this IServiceCollection serviceCollection, IPsConfig config)
        {
            return serviceCollection.AddSingleton(provider => config);
        }
    }
}