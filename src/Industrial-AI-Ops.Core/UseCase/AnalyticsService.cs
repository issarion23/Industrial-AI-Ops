using CSharpFunctionalExtensions;
using Industrial_AI_Ops.Core.Common.Result;
using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Ports.Repository;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.Extensions.Logging;

namespace Industrial_AI_Ops.Core.UseCase;

public class AnalyticsService : IAnalyticsService
{
    private readonly IEquipmentRepository _equipmentRepo;
    private readonly ILogger<AnalyticsService> _logger;

    public AnalyticsService(IEquipmentRepository equipmentRepo, ILogger<AnalyticsService> logger)
    {
        _equipmentRepo = equipmentRepo;
        _logger = logger;
    }

    public async Task<Result<EquipmentHealthTrendResponse>> GetEquipmentHealthTrend(string equipmentId, int days)
    {
        try
        {
            var equipment = await _equipmentRepo.GetEquipmentById(equipmentId);

            var result = new EquipmentHealthTrendResponse
            {
                EquipmentId = equipmentId,
                EquipmentName = equipment.Name,
                CurrentHealthScore = equipment.HealthScore,
                Trend = "stable", // Would calculate based on historical data
                PeriodDays = days,
                Timestamp = DateTime.UtcNow
            };

            return ResultFactory.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ResultFactory.Failure<EquipmentHealthTrendResponse>(ErrorCode.NotFound, ex.Message);
        }
        
    }
}