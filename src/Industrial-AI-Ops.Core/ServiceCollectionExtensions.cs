using Industrial_AI_Ops.Core.Common.Mapper;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Industrial_AI_Ops.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMapper(this IServiceCollection services)
    {
        services.AddSingleton(() =>
        {
            var config = new TypeAdapterConfig();

            new RegisterMapper().Register(config);

            return config;
        });

        return services;
    } 
}