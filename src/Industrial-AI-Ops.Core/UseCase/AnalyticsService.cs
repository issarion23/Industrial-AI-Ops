using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Ports.Repository;
using Industrial_AI_Ops.Core.Ports.UseCase;

namespace Industrial_AI_Ops.Core.UseCase;

public class AnalyticsService : IAnalyticsService
{
    private readonly IEquipmentRepository _equipmentRepo;

    public AnalyticsService(IEquipmentRepository equipmentRepo)
    {
        _equipmentRepo = equipmentRepo;
    }

    public async Task<EquipmentHealthTrendResponse> GetEquipmentHealthTrend(string equipmentId, int days)
    {
        var equipment = await _equipmentRepo.GetEquipmentById(equipmentId);

        return new EquipmentHealthTrendResponse
        {
            EquipmentId = equipmentId,
            EquipmentName = equipment.Name,
            CurrentHealthScore = equipment.HealthScore,
            Trend = "stable", // Would calculate based on historical data
            PeriodDays = days,
            Timestamp = DateTime.UtcNow
        };
    }
}