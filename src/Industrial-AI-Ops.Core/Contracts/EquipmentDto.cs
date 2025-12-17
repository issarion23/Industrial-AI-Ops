using Industrial_AI_Ops.Core.Models;

namespace Industrial_AI_Ops.Core.Contracts;

public record EquipmentDto(
    string Id,
    string Name,
    EquipmentType Type,
    string Location,
    EquipmentStatus Status,
    DateTime InstallationDate,
    DateTime? LastMaintenanceDate,
    double HealthScore = 100.0,
    List<PumpSensorData>? PumpData = null,
    List<CompressorSensorData>? CompressorData = null,
    List<TurbineSensorData>? TurbineData = null,
    List<MaintenancePrediction>? Predictions = null
)
{
    public List<PumpSensorData> PumpData { get; init; } = PumpData ?? new();
    public List<CompressorSensorData> CompressorData { get; init; } = CompressorData ?? new();
    public List<TurbineSensorData> TurbineData { get; init; } = TurbineData ?? new();
    public List<MaintenancePrediction> Predictions { get; init; } = Predictions ?? new();
}