using System.ComponentModel.DataAnnotations.Schema;
using Industrial_AI_Ops.Core.Models.Enums;

namespace Industrial_AI_Ops.Core.Models;

[Table("MaintenancePrediction")]
public class MaintenancePrediction
{
    public string Id { get; set; }
    public string EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
    public DateTime PredictionDate { get; set; }
    public DateTime PredictedFailureDate { get; set; }
    public double Confidence { get; set; }
    public string FailureType { get; set; } = string.Empty;
    public RiskLevel RiskLevel { get; set; }
    public string Recommendation { get; set; } = string.Empty;
    public bool IsAcknowledged { get; set; }
}