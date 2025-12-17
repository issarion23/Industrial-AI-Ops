using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.Enums;

namespace Industrial_AI_Ops.Core.Contracts;

public record MaintenancePredictionDto(
    int Id,
    int EquipmentId,
    Equipment Equipment,
    DateTime PredictionDate,
    DateTime PredictedFailureDate,
    double Confidence,
    string FailureType,
    RiskLevel RiskLevel,
    string Recommendation,
    bool IsAcknowledged);