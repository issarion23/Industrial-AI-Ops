using Industrial_AI_Ops.Api.Features.Swagger;
using Industrial_AI_Ops.Api.Features.Versioning;
using Industrial_AI_Ops.Api.Features.WebApi;
using Industrial_AI_Ops.Api.Options;
using Industrial_AI_Ops.Core;
using Industrial_AI_Ops.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var webHostOptions = new WebHostOptions(
    instanceName: builder.Configuration.GetValue<string>($"{WebHostOptions.SectionName}:InstanceName"),
    webAddress: builder.Configuration.GetValue<string>($"{WebHostOptions.SectionName}:WebAddress"));

try
{
    Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(Log.Logger);

    builder.ConfigureHostVersioning();

    builder.ConfigureWebHost();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:5174") // Vite dev server ports
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });

    builder.Services.ConfigureInfrastructure(builder.Configuration, "");
    builder.Services.ConfigureMapper();

    var app = builder.Build();

    Log.Information("{Msg} ({ApplicationName})...",
        "",
        webHostOptions.InstanceName);

    app.UseConfiguredSwagger();
    app.UseConfiguredVersioning();
    app.UseHttpsRedirection();
    // app.UseMiddleware<LoggingMiddleware>();
    // app.UseMiddleware<ExceptionHandleMiddleware>();
    app.UseCors("AllowFrontend");
    app.MapHealthChecks("/healthcheck");

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "({ApplicationName})!",
        webHostOptions.InstanceName);
}
finally
{
    Log.Information("{Msg}!", "");
    await Log.CloseAndFlushAsync();
}