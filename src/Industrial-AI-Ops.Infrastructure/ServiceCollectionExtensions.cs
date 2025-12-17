using Microsoft.Extensions.DependencyInjection;

namespace Industrial_AI_Ops.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDatabase(this IServiceCollection services)
    {
        return services;
    }
}