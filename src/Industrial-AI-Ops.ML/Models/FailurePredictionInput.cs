using Microsoft.ML.Data;

namespace Industrial_AI_Ops.ML.Models;

public class FailurePredictionInput
{
    public float HealthScore { get; set; }
    public float AnomalyScore { get; set; }
    public float DaysSinceLastMaintenance { get; set; }
    public float AvgVibration { get; set; }
    public float AvgTemperature { get; set; }
    public float AvgPressure { get; set; }
    public float OperatingHours { get; set; }
}

public class FailurePrediction
{
    [ColumnName("Score")] public float DaysToFailure { get; set; }
    [ColumnName("PredictedLabel")] public float RiskScore { get; set; }
}