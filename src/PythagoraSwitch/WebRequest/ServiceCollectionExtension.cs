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
            serviceCollection.AddSingleton<IPsRequestQueue, PsRequestQueue>();
            serviceCollection.AddSingleton<IPsWebRequesting, PsWebRequester>();
            serviceCollection.AddSingleton<IPsWebRequester, PsWebRequester>();
            serviceCollection.AddSingleton<IPsRecorder, PsRecorder>();
            serviceCollection.AddSingleton<IWebRequestExporter, WebRequestExporter>();
            return serviceCollection;
        }

        public static IServiceCollection AddPythagoraSwitchConfig(this IServiceCollection serviceCollection, IPsConfig config)
        {
            return serviceCollection.AddSingleton(provider => config);
        }
    }
}