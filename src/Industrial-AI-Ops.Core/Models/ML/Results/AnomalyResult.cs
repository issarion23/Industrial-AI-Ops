namespace Industrial_AI_Ops.Core.Models.ML.Results;

public class AnomalyResult
{
    public bool IsAnomaly { get; set; }
    public float AnomalyScore { get; set; }
    public float Confidence { get; set; }
    public Dictionary<string, float> FeatureImportance { get; set; } = new();
    public string AnomalyType { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
}