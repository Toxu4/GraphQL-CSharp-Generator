using System;
using Microsoft.Extensions.DependencyInjection;

namespace Toxu4.GraphQl.Client
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddGraphQlClient(this IServiceCollection serviceCollection, Action<GraphQlApiSettings> settings, Action<IHttpClientBuilder> httpClientBuilder = null)
        {
            serviceCollection.Configure(settings);    
            
            var builder =  serviceCollection 
                .AddHttpClient<IGraphQlQueryExecutor, GraphQlQueryExecutor>();
            
            httpClientBuilder?.Invoke(builder);
            
            return serviceCollection;
        }
    }
}