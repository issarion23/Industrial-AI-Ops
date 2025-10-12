using Microsoft.ML.Data;

namespace Industrial_AI_Ops.ML.Models;

public class PumpMlInput
{
    [LoadColumn(0)] public float SuctionPressure { get; set; }
    [LoadColumn(1)] public float DischargePressure { get; set; }
    [LoadColumn(2)] public float FlowRate { get; set; }
    [LoadColumn(3)] public float Temperature { get; set; }
    [LoadColumn(4)] public float BearingTemperature { get; set; }
    [LoadColumn(5)] public float VibrationX { get; set; }
    [LoadColumn(6)] public float VibrationY { get; set; }
    [LoadColumn(7)] public float VibrationZ { get; set; }
    [LoadColumn(8)] public float MotorCurrent { get; set; }
    [LoadColumn(9)] public float PowerConsumption { get; set; }
    [LoadColumn(10)] public float Rpm { get; set; }
    [LoadColumn(11)] public float Efficiency { get; set; }
}

public class AnomalyPrediction
{
    [ColumnName("PredictedLabel")] public bool IsAnomaly { get; set; }
    [ColumnName("Score")] public float AnomalyScore { get; set; }
}