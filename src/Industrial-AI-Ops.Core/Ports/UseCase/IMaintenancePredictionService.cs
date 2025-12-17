using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.Enums;

namespace Industrial_AI_Ops.Core.Ports.UseCase;

public interface IMaintenancePredictionService
{
    Task<List<MaintenancePrediction>> GetMaintenancePrediction(
        string? equipmentId = null,
        RiskLevel? riskLevel = null,
        bool? acknowledged = null);

    Task CreateMaintenancePrediction(MaintenancePredictionDto request);

    Task<PredictMaintenanceResponse> PredictMaintenance(string equipmentId);

    Task AcknowledgePrediction(string id);
}