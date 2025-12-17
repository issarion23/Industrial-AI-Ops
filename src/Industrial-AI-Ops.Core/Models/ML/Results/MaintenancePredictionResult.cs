namespace Industrial_AI_Ops.Core.Models.ML.Results;

public class MaintenancePredictionResult
{
    public float DaysToFailure { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string RecommendedAction { get; set; } = string.Empty;
    public float ConfidenceScore { get; set; }
}