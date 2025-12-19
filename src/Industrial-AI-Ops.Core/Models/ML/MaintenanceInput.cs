using Microsoft.ML.Data;

namespace Industrial_AI_Ops.Core.Models.ML;

public class MaintenanceInput
{
    public float HealthScore { get; set; }
    public float AnomalyScore { get; set; }
    public float DaysSinceLastMaintenance { get; set; }
    public float AvgVibration { get; set; }
    public float AvgTemperature { get; set; }
    public float OperatingHours { get; set; }
}

public class MaintenanceOutput
{
    [ColumnName("Score")] public float DaysToFailure { get; set; }
}
