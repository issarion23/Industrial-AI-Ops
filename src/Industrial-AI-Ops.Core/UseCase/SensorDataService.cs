using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.ML.Results;
using Industrial_AI_Ops.Core.Ports;
using Industrial_AI_Ops.Core.Ports.Repository;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Industrial_AI_Ops.Core.UseCase;

public class SensorDataService : ISensorDataService
{
    private readonly ISensorDataRepository _repo;
    private readonly ILogger<SensorDataService> _logger;
    private readonly IAnomalyDetectionService _anomalyDetectionService;

    public SensorDataService(
        ISensorDataRepository repo, 
        ILogger<SensorDataService> logger, 
        IAnomalyDetectionService anomalyDetectionService)
    {
        _repo = repo;
        _logger = logger;
        _anomalyDetectionService = anomalyDetectionService;
    }
    
    public async Task<List<PumpSensorData>> GetPumpSensorData(
        string? equipmentId,
        DateTime? startDate,
        DateTime? endDate,
        int limit)
    {
        try
        {
            return await _repo.GetPumpSensorDataAsync(equipmentId, startDate, endDate, limit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task AddPumpSensorData(PumpSensorDataDto request)
    {
        try
        {
            _repo.AddPumpSensorDataAsync(request.Adapt<PumpSensorData>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<AnomalyResult> DetectPumpAnomaly(string id)
    {
        try
        {
            var data = await _repo.GetPumpSensorDataByIdAsync(id);
            
            return await _anomalyDetectionService.DetectPumpAnomalyAsync(data);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
    
    public async Task<List<CompressorSensorData>> GetCompressorSensorData(
        string? equipmentId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 1000)
    {
        try
        {
            return await _repo.GetCompressorSensorDataAsync(equipmentId, startDate, endDate, limit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
    
    public async Task AddCompressorSensorData(CompressorSensorDataDto request)
    {
        try
        {
            _repo.AddCompressorSensorDataAsync(request.Adapt<CompressorSensorData>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
    
    public async Task<AnomalyResult> DetectCompressorAnomaly(string id)
    {
        try
        {
            var data = await _repo.GetCompressorSensorDataByIdAsync(id);
            
            return await _anomalyDetectionService.DetectCompressorAnomalyAsync(data);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
    
    public async Task<List<TurbineSensorData>> GetTurbineSensorData(
        string? equipmentId,
        DateTime? startDate,
        DateTime? endDate,
        int limit)
    {
        try
        {
            return await _repo.GetTurbineSensorDataAsync(equipmentId, startDate, endDate, limit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
    
    public async Task AddTurbineSensorData(TurbineSensorDataDto request)
    {
        try
        {
            _repo.AddTurbineSensorDataAsync(request.Adapt<TurbineSensorData>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
    
    public async Task<AnomalyResult> DetectTurbineAnomaly(string id)
    {
        try
        {
            var data = await _repo.GetTurbineSensorDataByIdAsync(id);
            
            return await _anomalyDetectionService.DetectTurbineAnomalyAsync(data);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}