using Microsoft.ML.Data;

namespace Industrial_AI_Ops.ML.Models;

public class CompressorMlInput
{
    [LoadColumn(0)] public float InletPressure { get; set; }
    [LoadColumn(1)] public float OutletPressure { get; set; }
    [LoadColumn(2)] public float InletTemperature { get; set; }
    [LoadColumn(3)] public float OutletTemperature { get; set; }
    [LoadColumn(4)] public float MassFlowRate { get; set; }
    [LoadColumn(5)] public float VibrationAxial { get; set; }
    [LoadColumn(6)] public float VibrationRadial { get; set; }
    [LoadColumn(7)] public float BearingTemp1 { get; set; }
    [LoadColumn(8)] public float BearingTemp2 { get; set; }
    [LoadColumn(9)] public float PowerConsumption { get; set; }
    [LoadColumn(10)] public float Rpm { get; set; }
    [LoadColumn(11)] public float LubOilPressure { get; set; }
    [LoadColumn(12)] public float SurgeMargin { get; set; }
}