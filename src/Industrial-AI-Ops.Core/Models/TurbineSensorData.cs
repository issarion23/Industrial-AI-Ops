namespace Industrial_AI_Ops.Core.Models;

public class TurbineSensorData : BaseSensorData
{
    public double? InletPressure { get; set; }
    public double? InletTemperature { get; set; }
    public double? ExhaustTemperature { get; set; }
    public double? FuelGasFlowRate { get; set; }
    public double? PowerOutput { get; set; }
    public double? Rpm { get; set; }
    public double? VibrationBearing1 { get; set; }
    public double? VibrationBearing2 { get; set; }
    public double? BearingTemp1 { get; set; }
    public double? BearingTemp2 { get; set; }
    public double? LubOilPressure { get; set; }
    public double? ThermalEfficiency { get; set; }
    public double? NOxEmission { get; set; }
}