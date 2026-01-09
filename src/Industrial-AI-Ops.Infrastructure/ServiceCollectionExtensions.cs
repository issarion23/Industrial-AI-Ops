using Industrial_AI_Ops.Core.Ports.LLM;
using Industrial_AI_Ops.Core.Ports.ML;
using Industrial_AI_Ops.Core.Ports.RAG;
using Industrial_AI_Ops.Core.Ports.Repository;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Industrial_AI_Ops.Core.UseCase;
using Industrial_AI_Ops.Infrastructure.AI;
using Industrial_AI_Ops.Infrastructure.Persistence;
using Industrial_AI_Ops.Infrastructure.Repository;
using Industrial_AI_Ops.ML;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Industrial_AI_Ops.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddRepositories();
        services.AddUseCases();
        services.AddMachineLearning();
        services.AddAIServices();
        services.AddPersistence(configuration);
        
        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<IMaintenancePredictionRepository, MaintenancePredictionRepository>();
        services.AddScoped<ISensorDataRepository, SensorDataRepository>();
    }

    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<IMaintenancePredictionService, MaintenancePredictionService>();
        services.AddScoped<IMlModelManagementService, MlModelManagementService>();
        services.AddScoped<ISensorDataService, SensorDataService>();
    }

    private static void AddMachineLearning(this IServiceCollection services)
    {
        services.AddScoped<IAnomalyDetectionService, AnomalyDetectionService>();
        services.AddScoped<IMlModelTrainService, MlModelTrainService>();
    }

    private static void AddAIServices(this IServiceCollection services)
    {
        // LLM Services
        services.AddScoped<ILlmService, OpenAiLlmService>();
        
        // RAG Services
        services.AddScoped<IDocumentRetriever, InMemoryDocumentRetriever>();
        services.AddScoped<IRagService, RagService>();
    }

    private static void AddPersistence(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<AppDbContext>();
    }
}