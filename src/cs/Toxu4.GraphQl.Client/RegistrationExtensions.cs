using System;
using Microsoft.Extensions.DependencyInjection;

namespace Toxu4.GraphQl.Client
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddGraphQlClient(this IServiceCollection serviceCollection, Action<GraphQlApiSettings> settings)
        {
            serviceCollection.Configure(settings);     
            serviceCollection
                .AddHttpClient<IGraphQlQueryExecutor, GraphQlQueryExecutor>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));              
            
            return serviceCollection;
        }
    }
}