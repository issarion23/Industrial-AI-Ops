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
    
    public MlModelStatusResponse GetModelsStatus()
    {
        var isLoaded = _anomalyDetectionService.AreModelsLoaded();
        var isValidated = _modelTrainService.ValidateModelsAsync();

        return new MlModelStatusResponse
        {
            ModelsLoaded = isLoaded,
            ModelsValidated = isValidated,
            PumpModel = _modelTrainService.GetPumpModel() != null,
            CompressorModel = _modelTrainService.GetCompressorModel() != null,
            TurbineModel = _modelTrainService.GetTurbineModel() != null,
            MaintenanceModel = _modelTrainService.GetMaintenanceModel() != null,
            Timestamp = DateTime.UtcNow
        };
    }

    public async Task InitializeModels()
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
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to initialize models. Message: {Message}", exception.Message);
            throw;
        }
    }

    public async Task RetrainModels()
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrain models");
            throw;
        }
    }
}