using CSharpFunctionalExtensions;
using Industrial_AI_Ops.Core.Common.Result;
using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.Enums;
using Industrial_AI_Ops.Core.Models.ML;
using Industrial_AI_Ops.Core.Ports;
using Industrial_AI_Ops.Core.Ports.Repository;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Industrial_AI_Ops.Core.UseCase;

public class MaintenancePredictionService : IMaintenancePredictionService
{
    private readonly IMaintenancePredictionRepository _repo;
    private readonly ILogger<MaintenancePredictionService> _logger;
    private readonly IEquipmentRepository _equipmentRepo;
    private readonly ISensorDataRepository _sensorDataRepo;
    private readonly IAnomalyDetectionService _anomalyDetectionService;
    
    private const int PumpDataLimit = 100;
    private const int CompressorDataLimit = 100;
    private const int TurbineDataLimit = 100;
    
    public MaintenancePredictionService(
        IMaintenancePredictionRepository repo, 
        ILogger<MaintenancePredictionService> logger, 
        IEquipmentRepository equipmentRepo, ISensorDataRepository sensorDataRepo, IAnomalyDetectionService anomalyDetectionService)
    {
        _repo = repo;
        _logger = logger;
        _equipmentRepo = equipmentRepo;
        _sensorDataRepo = sensorDataRepo;
        _anomalyDetectionService = anomalyDetectionService;
    }

    public async Task<Result<List<MaintenancePrediction>>> GetMaintenancePrediction(
        string? equipmentId = null,
        RiskLevel? riskLevel = null,
        bool? acknowledged = null)
    {
        try
        {
            return ResultFactory.Success(await _repo.GetMaintenancePredictionAsync(equipmentId, riskLevel, acknowledged));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure<List<MaintenancePrediction>>(ErrorCode.Validation, ex.Message);
        }
    }

    public async Task<Result> CreateMaintenancePrediction(MaintenancePredictionDto request)
    {
        try
        {
            await _repo.AddMaintenancePrediction(request.Adapt<MaintenancePrediction>());
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure(ErrorCode.Validation, ex.Message);
        }
    }

    public async Task<Result<PredictMaintenanceResponse>> PredictMaintenance(string equipmentId)
    {
        try
        {
            var equipment = await _equipmentRepo.GetEquipmentById(equipmentId);
        
            if (equipment == null)
                throw new InvalidOperationException($"Equipment with ID {equipmentId} not found");

            // Calculate maintenance input based on recent sensor data and equipment health
            var input = new MaintenanceInput
            {
                HealthScore = (float)equipment.HealthScore,
                AnomalyScore = await CalculateAverageAnomalyScore(equipmentId),
                DaysSinceLastMaintenance = equipment.LastMaintenanceDate.HasValue
                    ? (float)(DateTime.UtcNow - equipment.LastMaintenanceDate.Value).TotalDays
                    : 365,
                AvgVibration = await CalculateAverageVibration(equipmentId, equipment.Type),
                AvgTemperature = await CalculateAverageTemperature(equipmentId, equipment.Type),
                OperatingHours = (float)(DateTime.UtcNow - equipment.InstallationDate).TotalHours
            };
            
            var result = await _anomalyDetectionService.PredictMaintenanceAsync(input);

            return ResultFactory.Success(new PredictMaintenanceResponse
            {
                EquipmentId = equipmentId,
                EquipmentName = equipment.Name,
                CurrentHealthScore = equipment.HealthScore,
                Prediction = result,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Maintenance model may not be loaded yet. Message: {Message}", ex.Message);
            return ResultFactory.Failure<PredictMaintenanceResponse>(ErrorCode.Validation, ex.Message);
        }
    }

    public async Task<Result> AcknowledgePrediction(string id)
    {
        try
        {
            await _repo.UpdateMaintenancePredictionAcknowledge(id);
            return ResultFactory.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultFactory.Failure(ErrorCode.Validation, e.Message);
        }
    }
    
    
    private async Task<float> CalculateAverageAnomalyScore(string equipmentId)
    {
        // This is a simplified implementation
        // In production, you'd store anomaly scores and calculate average
        return 5.0f; // Default value
    }

    private async Task<float> CalculateAverageVibration(string equipmentId, EquipmentType type)
    {
        return type switch
        {
            EquipmentType.Pump => await CalculatePumpAverageVibration(equipmentId),
            EquipmentType.Compressor => await CalculateCompressorAverageVibration(equipmentId),
            EquipmentType.Turbine => await CalculateTurbineAverageVibration(equipmentId),
            _ => 0f
        };
    }   

    private async Task<float> CalculatePumpAverageVibration(string equipmentId)
    {
        var recentData = await _sensorDataRepo.GetPumpDataForCalculateVibration(equipmentId, PumpDataLimit);

        if (!recentData.Any()) return 0f;

        var avgX = recentData.Average(d => d.VibrationX ?? 0);
        var avgY = recentData.Average(d => d.VibrationY ?? 0);
        var avgZ = recentData.Average(d => d.VibrationZ ?? 0);

        return (float)((avgX + avgY + avgZ) / 3);
    }

    private async Task<float> CalculateCompressorAverageVibration(string equipmentId)
    {
        var recentData = await _sensorDataRepo.GetCompressorDataForCalculateVibration(equipmentId, CompressorDataLimit);

        if (!recentData.Any()) return 0f;

        var avgAxial = recentData.Average(d => d.VibrationAxial ?? 0);
        var avgRadial = recentData.Average(d => d.VibrationRadial ?? 0);

        return (float)((avgAxial + avgRadial) / 2);
    }

    private async Task<float> CalculateTurbineAverageVibration(string equipmentId)
    {
        var recentData = await _sensorDataRepo.GetTurbineDataForCalculateVibration(equipmentId, TurbineDataLimit);

        if (!recentData.Any()) return 0f;

        var avgB1 = recentData.Average(d => d.VibrationBearing1 ?? 0);
        var avgB2 = recentData.Average(d => d.VibrationBearing2 ?? 0);

        return (float)((avgB1 + avgB2) / 2);
    }

    private async Task<float> CalculateAverageTemperature(string equipmentId, EquipmentType type)
    {
        return type switch
        {
            EquipmentType.Pump => await CalculatePumpAverageTemperature(equipmentId, PumpDataLimit),
            EquipmentType.Compressor => await CalculateCompressorAverageTemperature(equipmentId, CompressorDataLimit),
            EquipmentType.Turbine => await CalculateTurbineAverageTemperature(equipmentId, TurbineDataLimit),
            _ => 0f
        };
    }

    private async Task<float> CalculatePumpAverageTemperature(string equipmentId, int limit)
    {
        var recentData = await _sensorDataRepo.GetPumpDataForCalculateVibration(equipmentId, limit);

        return recentData.Any() 
            ? (float)recentData.Average(d => d.BearingTemperature ?? 0) 
            : 0f;
    }

    private async Task<float> CalculateCompressorAverageTemperature(string equipmentId, int limit)
    {
        var recentData = await _sensorDataRepo.GetCompressorDataForCalculateVibration(equipmentId, limit);

        if (!recentData.Any()) return 0f;

        return (float)((recentData.Average(d => d.BearingTemp1 ?? 0) + 
                       recentData.Average(d => d.BearingTemp2 ?? 0)) / 2);
    }

    private async Task<float> CalculateTurbineAverageTemperature(string equipmentId, int limit)
    {
        var recentData = await _sensorDataRepo.GetTurbineDataForCalculateVibration(equipmentId, limit);

        if (!recentData.Any()) return 0f;

        return (float)((recentData.Average(d => d.BearingTemp1 ?? 0) + 
                       recentData.Average(d => d.BearingTemp2 ?? 0)) / 2);
    }
}