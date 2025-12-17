using Industrial_AI_Ops.Core.Contracts.Response;

namespace Industrial_AI_Ops.Core.Ports.UseCase;

public interface IAnalyticsService
{
    Task<EquipmentHealthTrendResponse> GetEquipmentHealthTrend(string equipmentId, int days);
}