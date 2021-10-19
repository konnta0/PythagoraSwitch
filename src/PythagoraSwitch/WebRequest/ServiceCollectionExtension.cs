using System;
using Microsoft.Extensions.DependencyInjection;
using PythagoraSwitch.Recorder;
using PythagoraSwitch.Recorder.Interfaces;
using PythagoraSwitch.WebRequest.Interfaces;
using IConfig = PythagoraSwitch.WebRequest.Interfaces.IConfig;

namespace PythagoraSwitch.WebRequest
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPythagoraSwitch(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            serviceCollection.AddSingleton<ISerializer, JsonSerializer>();
            serviceCollection.AddSingleton<IRequestQueue, RequestQueue>();
            serviceCollection.AddSingleton<IWebRequestHandler, WebRequestHandler>();
            serviceCollection.AddSingleton<IRecorder, Recorder.Recorder>();
            serviceCollection.AddSingleton<IWebRequestExporter, WebRequestExporter>();
            return serviceCollection;
        }

        public static IServiceCollection AddPythagoraSwitchConfig(this IServiceCollection serviceCollection, IConfig config)
        {
            return serviceCollection.AddSingleton(provider => config);
        }
    }
}