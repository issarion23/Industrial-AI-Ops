using CSharpFunctionalExtensions;
using Industrial_AI_Ops.Core.Contracts.Response;

namespace Industrial_AI_Ops.Core.Ports.UseCase;

public interface IAnalyticsService
{
    Task<Result<EquipmentHealthTrendResponse>> GetEquipmentHealthTrend(string equipmentId, int days);
}