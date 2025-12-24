using CSharpFunctionalExtensions;
using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.Enums;

namespace Industrial_AI_Ops.Core.Ports.UseCase;

public interface IMaintenancePredictionService
{
    Task<Result<List<MaintenancePrediction>>> GetMaintenancePrediction(
        string? equipmentId = null,
        RiskLevel? riskLevel = null,
        bool? acknowledged = null);

    Task<Result> CreateMaintenancePrediction(MaintenancePredictionDto request);

    Task<Result<PredictMaintenanceResponse>> PredictMaintenance(string equipmentId);

    Task<Result> AcknowledgePrediction(string id);
}