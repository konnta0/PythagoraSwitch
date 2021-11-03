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
            return serviceCollection.AddPythagoraSwitch(new DefaultConfig(), new EmptyNetworkAccess(),
                new EmptyRequestInterceptor());
        }

        public static IServiceCollection AddPythagoraSwitch(this IServiceCollection serviceCollection, IConfig config)
        {
            return serviceCollection.AddPythagoraSwitch(config, new EmptyNetworkAccess(), new EmptyRequestInterceptor());
        }

        public static IServiceCollection AddPythagoraSwitch(this IServiceCollection serviceCollection, IConfig config, IWebRequestRecorderInterceptor recorderInterceptor)
        {
            return serviceCollection.AddPythagoraSwitch(config, new EmptyNetworkAccess(), recorderInterceptor);
        }

        public static IServiceCollection AddPythagoraSwitch(this IServiceCollection serviceCollection, IConfig config, INetworkAccess network, IWebRequestRecorderInterceptor recorderInterceptor)
        {
            serviceCollection.AddScoped(x => network ?? new EmptyNetworkAccess());
            serviceCollection.AddSingleton<IRequestQueue, RequestQueue>();
            serviceCollection.AddSingleton<IWebRequestHandler, WebRequestHandler>();
            serviceCollection.AddSingleton(x => config ?? new DefaultConfig());
            serviceCollection.AddScoped<IHttpClientFactory, HttpClientFactory>();
            serviceCollection.AddSingleton<ISerializer, JsonSerializer>();
            serviceCollection.AddSingleton<IWebRequestInterceptor, WebRequestInterceptor>();

            serviceCollection.AddSingleton<IRecorder, Recorder.Recorder>();
            serviceCollection.AddSingleton<IWebRequestExporter, WebRequestExporter>();
            serviceCollection.AddSingleton(x => recorderInterceptor ?? new EmptyRequestInterceptor());
            return serviceCollection;
        }
    }
}