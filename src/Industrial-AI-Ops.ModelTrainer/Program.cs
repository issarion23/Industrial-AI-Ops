using Industrial_AI_Ops.Core.Ports;
using Industrial_AI_Ops.ML;
using Industrial_AI_Ops.Core.Ports.Repository;
using Industrial_AI_Ops.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(cfg =>
    {
        cfg.AddJsonFile("appsettings.json", optional: true);
    })
    .ConfigureServices((ctx, services) =>
    {
        services.AddLogging(l => l.AddConsole());
        
        services.AddScoped<ISensorDataRepository, SensorDataRepository>();
        
        services.AddScoped<IMlModelTrainService, MlModelTrainService>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var trainer = scope.ServiceProvider.GetRequiredService<IMlModelTrainService>();

await trainer.InitializeAllModelsAsync();

Console.WriteLine("MODELS READY (./models)");