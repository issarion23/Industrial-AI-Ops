using CSharpFunctionalExtensions;
using Industrial_AI_Ops.Core.Common.Result;
using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.ML.Results;
using Industrial_AI_Ops.Core.Ports;
using Industrial_AI_Ops.Core.Ports.ML;
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
    
    public async Task<Result<List<PumpSensorData>>> GetPumpSensorData(
        string? equipmentId,
        DateTime? startDate,
        DateTime? endDate,
        int limit)
    {
        try
        {
            return ResultFactory.Success(await _repo.GetPumpSensorDataAsync(equipmentId, startDate, endDate, limit));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure<List<PumpSensorData>>(ErrorCode.NotFound, ex.Message);
        }
    }

    public async Task<Result> AddPumpSensorData(PumpSensorDataDto request)
    {
        try
        {
            _repo.AddPumpSensorDataAsync(request.Adapt<PumpSensorData>());
            
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure(ErrorCode.Validation, ex.Message);
        }
    }

    public async Task<Result<AnomalyResult>> DetectPumpAnomaly(string id)
    {
        try
        {
            var data = await _repo.GetPumpSensorDataByIdAsync(id);
            
            return ResultFactory.Success(await _anomalyDetectionService.DetectPumpAnomalyAsync(data));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure<AnomalyResult>(ErrorCode.Validation, ex.Message);
        }
    }
    
    public async Task<Result<List<CompressorSensorData>>> GetCompressorSensorData(
        string? equipmentId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 1000)
    {
        try
        {
            return ResultFactory.Success(await _repo.GetCompressorSensorDataAsync(equipmentId, startDate, endDate, limit));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure<List<CompressorSensorData>>(ErrorCode.NotFound, ex.Message);
        }
    }
    
    public async Task<Result> AddCompressorSensorData(CompressorSensorDataDto request)
    {
        try
        {
            _repo.AddCompressorSensorDataAsync(request.Adapt<CompressorSensorData>());

            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure(ErrorCode.Validation, ex.Message);
        }
    }
    
    public async Task<Result<AnomalyResult>> DetectCompressorAnomaly(string id)
    {
        try
        {
            var data = await _repo.GetCompressorSensorDataByIdAsync(id);
            
            return ResultFactory.Success(await _anomalyDetectionService.DetectCompressorAnomalyAsync(data));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure<AnomalyResult>(ErrorCode.Validation, ex.Message);
        }
    }
    
    public async Task<Result<List<TurbineSensorData>>> GetTurbineSensorData(
        string? equipmentId,
        DateTime? startDate,
        DateTime? endDate,
        int limit)
    {
        try
        {
            return ResultFactory.Success(await _repo.GetTurbineSensorDataAsync(equipmentId, startDate, endDate, limit));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure<List<TurbineSensorData>>(ErrorCode.NotFound, ex.Message);
        }
    }
    
    public async Task<Result> AddTurbineSensorData(TurbineSensorDataDto request)
    {
        try
        {
            _repo.AddTurbineSensorDataAsync(request.Adapt<TurbineSensorData>());

            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure(ErrorCode.Validation, ex.Message);
        }
    }
    
    public async Task<Result<AnomalyResult>> DetectTurbineAnomaly(string id)
    {
        try
        {
            var data = await _repo.GetTurbineSensorDataByIdAsync(id);
            
            return ResultFactory.Success(await _anomalyDetectionService.DetectTurbineAnomalyAsync(data));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure<AnomalyResult>(ErrorCode.Validation, ex.Message);
        }
    }
}