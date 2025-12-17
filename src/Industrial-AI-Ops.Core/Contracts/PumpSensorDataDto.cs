namespace Industrial_AI_Ops.Core.Contracts;

public record PumpSensorDataDto : BaseSensorDataDto
{
    public double? SuctionPressure { get; init; }
    public double? DischargePressure { get; init; }
    public double? DifferentialPressure { get; init; }
    public double? FlowRate { get; init; }
    public double? Temperature { get; init; }
    public double? BearingTemperature { get; init; }
    public double? VibrationX { get; init; }
    public double? VibrationY { get; init; }
    public double? VibrationZ { get; init; }
    public double? MotorCurrent { get; init; }
    public double? PowerConsumption { get; init; }
    public double? Rpm { get; init; }
    public double? Efficiency { get; init; }
}