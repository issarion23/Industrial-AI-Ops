namespace Industrial_AI_Ops.Core.Models.ML;

public class TurbineMlInput
{
    [LoadColumn(0)] public float InletPressure { get; set; }
    [LoadColumn(1)] public float InletTemperature { get; set; }
    [LoadColumn(2)] public float ExhaustTemperature { get; set; }
    [LoadColumn(3)] public float FuelGasFlowRate { get; set; }
    [LoadColumn(4)] public float PowerOutput { get; set; }
    [LoadColumn(5)] public float Rpm { get; set; }
    [LoadColumn(6)] public float VibrationBearing1 { get; set; }
    [LoadColumn(7)] public float VibrationBearing2 { get; set; }
    [LoadColumn(8)] public float BearingTemp1 { get; set; }
    [LoadColumn(9)] public float BearingTemp2 { get; set; }
    [LoadColumn(10)] public float LubOilPressure { get; set; }
    [LoadColumn(11)] public float ThermalEfficiency { get; set; }
    [LoadColumn(12)] public float NOxEmission { get; set; }
}