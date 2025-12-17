using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Ports;
using Microsoft.EntityFrameworkCore;

namespace Industrial_AI_Ops.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<Equipment> Equipment { get; set; }

    public DbSet<PumpSensorData> PumpSensorData { get; set; }
    
    public DbSet<CompressorSensorData> CompressorSensorData { get; set; }
    
    public DbSet<TurbineSensorData> TurbineSensorData { get; set; }
    
    public DbSet<MaintenancePrediction> MaintenancePredictions { get; set; }
}