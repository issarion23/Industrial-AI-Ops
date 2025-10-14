namespace Industrial_AI_Ops.Core.Models;

public class PumpSensorData : BaseSensorData
{
    public double? SuctionPressure { get; set; }
    public double? DischargePressure { get; set; }
    public double? DifferentialPressure { get; set; }
    public double? FlowRate { get; set; }
    public double? Temperature { get; set; }
    public double? BearingTemperature { get; set; }
    public double? VibrationX { get; set; }
    public double? VibrationY { get; set; }
    public double? VibrationZ { get; set; }
    public double? MotorCurrent { get; set; }
    public double? PowerConsumption { get; set; }
    public double? Rpm { get; set; }
    public double? Efficiency { get; set; }
}