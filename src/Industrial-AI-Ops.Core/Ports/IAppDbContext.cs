using Industrial_AI_Ops.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Industrial_AI_Ops.Core.Ports;

public interface IAppDbContext
{
    DbSet<PumpSensorData> PumpSensorData { get; set; }
    DbSet<CompressorSensorData> CompressorSensorData { get; set; }
    DbSet<TurbineSensorData> TurbineSensorData { get; set; }
}