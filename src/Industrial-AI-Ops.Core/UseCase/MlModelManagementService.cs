using CSharpFunctionalExtensions;
using Industrial_AI_Ops.Core.Common.Result;
using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Ports;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.Extensions.Logging;

namespace Industrial_AI_Ops.Core.UseCase;

public class MlModelManagementService : IMlModelManagementService
{
    private readonly IAnomalyDetectionService _anomalyDetectionService;
    private readonly IMlModelTrainService _modelTrainService;
    private readonly ILogger<MlModelManagementService> _logger;

    public MlModelManagementService(IAnomalyDetectionService anomalyDetectionService, IMlModelTrainService modelTrainService, ILogger<MlModelManagementService> logger)
    {
        _anomalyDetectionService = anomalyDetectionService;
        _modelTrainService = modelTrainService;
        _logger = logger;
    }
    
    public Result<MlModelStatusResponse> GetModelsStatus()
    {
        try
        {
            var isLoaded = _anomalyDetectionService.AreModelsLoaded();
            var isValidated = _modelTrainService.ValidateModelsAsync();
        
            var result = new MlModelStatusResponse
            {
                ModelsLoaded = isLoaded,
                ModelsValidated = isValidated,
                PumpModel = _modelTrainService.GetPumpModel() != null,
                CompressorModel = _modelTrainService.GetCompressorModel() != null,
                TurbineModel = _modelTrainService.GetTurbineModel() != null,
                MaintenanceModel = _modelTrainService.GetMaintenanceModel() != null,
                Timestamp = DateTime.UtcNow
            };

            return ResultFactory.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultFactory.Failure<MlModelStatusResponse>(ErrorCode.NotFound, e.Message);
        }
    }

    public async Task<Result> InitializeModels()
    {
        try
        {
            await _modelTrainService.InitializeAllModelsAsync();
            
            _anomalyDetectionService.LoadModels(
                _modelTrainService.GetPumpModel()!,
                _modelTrainService.GetCompressorModel()!,
                _modelTrainService.GetTurbineModel()!,
                _modelTrainService.GetMaintenanceModel()
            );

            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize models. Message: {Message}", ex.Message);
            return ResultFactory.Failure(ErrorCode.Validation, ex.Message);
        }
    }

    public async Task<Result> RetrainModels()
    {
        try
        {
            await _modelTrainService.RetrainAllModelsAsync();
            
            _anomalyDetectionService.LoadModels(
                _modelTrainService.GetPumpModel()!,
                _modelTrainService.GetCompressorModel()!,
                _modelTrainService.GetTurbineModel()!,
                _modelTrainService.GetMaintenanceModel()
            );

            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrain models");
            return ResultFactory.Failure(ErrorCode.Validation, ex.Message);
        }
    }
}