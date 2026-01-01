using Industrial_AI_Ops.Core.Ports;
using Industrial_AI_Ops.Core.Ports.ML;
using Industrial_AI_Ops.Core.Ports.Repository;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Industrial_AI_Ops.Core.UseCase;
using Industrial_AI_Ops.Infrastructure.Persistence;
using Industrial_AI_Ops.Infrastructure.Repository;
using Industrial_AI_Ops.ML;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Industrial_AI_Ops.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration, string environmentName)
    {
        services.AddServices();
        services.AddPersistence(configuration, environmentName);
        
        return services;
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<IMaintenancePredictionRepository, MaintenancePredictionRepository>();
        services.AddScoped<ISensorDataRepository, SensorDataRepository>();

        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<IMaintenancePredictionService, MaintenancePredictionService>();
        services.AddScoped<IMlModelManagementService, MlModelManagementService>();
        services.AddScoped<ISensorDataService, SensorDataService>();

        services.AddScoped<AppDbContext>();

        services.AddScoped<IAnomalyDetectionService, AnomalyDetectionService>();
        services.AddScoped<IMlModelTrainService, MlModelTrainService>();
    }
    
    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration, string environmentName)
    {
        // var connectionString = configuration.GetConnectionString("IndustrialAIOps")!;
        //
        // services.AddDbContext<AppDbContext>(options =>
        // {
        //     options.UseNpgsql(connectionString,
        //         sqlOptions =>
        //         {
        //             sqlOptions.EnableRetryOnFailure(
        //                 3,
        //                 TimeSpan.FromSeconds(10),
        //                 null);
        //         });
        // });
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
    }
}