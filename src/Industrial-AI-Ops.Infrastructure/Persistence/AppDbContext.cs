using Industrial_AI_Ops.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Industrial_AI_Ops.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<PumpSensorData> PumpSensorData { get; set; }
    
    public DbSet<CompressorSensorData> CompressorSensorData { get; set; }
    
    public DbSet<TurbineSensorData> TurbineSensorData { get; set; }
}