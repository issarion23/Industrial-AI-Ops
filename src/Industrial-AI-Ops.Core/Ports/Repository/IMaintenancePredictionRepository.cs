using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.Enums;

namespace Industrial_AI_Ops.Core.Ports.Repository;

public interface IMaintenancePredictionRepository
{
    Task<List<MaintenancePrediction>> GetMaintenancePredictionAsync(
        string? equipmentId = null,
        RiskLevel? riskLevel = null,
        bool? acknowledged = null);

    Task AddMaintenancePrediction(MaintenancePrediction prediction);

    Task<int> GetMaintenancePredictionCountByRiskLevel(RiskLevel riskLevel);
    Task UpdateMaintenancePredictionAcknowledge(string id);
}