namespace Industrial_AI_Ops.Core.Models;

public class CompressorSensorData : BaseSensorData
{
    public double? InletPressure { get; set; }
    public double? OutletPressure { get; set; }
    public double? InletTemperature { get; set; }
    public double? OutletTemperature { get; set; }
    public double? MassFlowRate { get; set; }
    public double? VibrationAxial { get; set; }
    public double? VibrationRadial { get; set; }
    public double? BearingTemp1 { get; set; }
    public double? BearingTemp2 { get; set; }
    public double? PowerConsumption { get; set; }
    public double? Rpm { get; set; }
    public double? LubOilPressure { get; set; }
    public double? LubOilTemperature { get; set; }
    public double? SurgeMargin { get; set; }
}