using CSharpFunctionalExtensions;
using Industrial_AI_Ops.Core.Common.Result;
using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.Enums;
using Industrial_AI_Ops.Core.Ports.Repository;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.Extensions.Logging;

namespace Industrial_AI_Ops.Core.UseCase;

public class DashboardService : IDashboardService
{
    private readonly IEquipmentRepository _equipmentRepo;
    private readonly IMaintenancePredictionRepository _maintenanceRepo;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(
        IEquipmentRepository equipmentRepo, 
        IMaintenancePredictionRepository maintenanceRepo, 
        ILogger<DashboardService> logger)
    {
        _equipmentRepo = equipmentRepo;
        _maintenanceRepo = maintenanceRepo;
        _logger = logger;
    }

    public async Task<Result<DashboardSummaryResponse>> GetDashboardSummary()
    {
        try
        {
            var totalEquipment = await _equipmentRepo.GetEquipmentCountByStatus();
            var operationalCount = await _equipmentRepo.GetEquipmentCountByStatus(EquipmentStatus.Operational);
            var warningCount = await _equipmentRepo.GetEquipmentCountByStatus(EquipmentStatus.Warning);
            var criticalCount = await _equipmentRepo.GetEquipmentCountByStatus(EquipmentStatus.Critical);
            var offlineCount = await _equipmentRepo.GetEquipmentCountByStatus(EquipmentStatus.Offline);

            var criticalPredictions = await _maintenanceRepo.GetMaintenancePredictionCountByRiskLevel(RiskLevel.Critical);

            var avgHealthScore = await _equipmentRepo.GetEquipmentAverageHealthScore();

            var result = new DashboardSummaryResponse
            {
                TotalEquipment = totalEquipment,
                EquipmentStatus = new EquipmentStatusResponse
                {
                    Operational = operationalCount,
                    Warning = warningCount,
                    Critical = criticalCount,
                    Offline = offlineCount
                },
                CriticalAlerts = criticalPredictions,
                AverageHealthScore = Math.Round(avgHealthScore, 2),
                Timestamp = DateTime.UtcNow
            };

            return ResultFactory.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure<DashboardSummaryResponse>(ErrorCode.NotFound, ex.Message);
        }
    }
}