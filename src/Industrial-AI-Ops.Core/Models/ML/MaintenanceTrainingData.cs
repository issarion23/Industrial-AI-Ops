using Microsoft.ML.Data;

namespace Industrial_AI_Ops.Core.Models.ML;

// <summary>
/// Training data model for supervised maintenance prediction
/// Combines input features with target label (DaysToFailure)
/// </summary>
public class MaintenanceTrainingData
{
    // Input Features
    public float HealthScore { get; set; }
    public float AnomalyScore { get; set; }
    public float DaysSinceLastMaintenance { get; set; }
    public float AvgVibration { get; set; }
    public float AvgTemperature { get; set; }
    public float OperatingHours { get; set; }
    
    // Target Label (for supervised learning)
    // ВАЖНО: Используем ColumnName("Label") для ML.NET
    [ColumnName("Label")]
    public float DaysToFailure { get; set; }
}