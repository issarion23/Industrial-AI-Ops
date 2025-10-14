namespace Industrial_AI_Ops.Core.Models;

public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public EquipmentType Type { get; set; }
    public string Location { get; set; } = string.Empty;
    public EquipmentStatus Status { get; set; }
    public DateTime InstallationDate { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public double HealthScore { get; set; } = 100.0;
    
    public List<PumpSensorData> PumpData { get; set; } = new();
    public List<CompressorSensorData> CompressorData { get; set; } = new();
    public List<TurbineSensorData> TurbineData { get; set; } = new();
    public List<MaintenancePrediction> Predictions { get; set; } = new();
}

public enum EquipmentType
{
    Pump,
    Compressor,
    Turbine,
    Furnace,
    HeatExchanger
}

public enum EquipmentStatus
{
    Operational,
    Warning,
    Critical,
    Maintenance,
    Offline
}