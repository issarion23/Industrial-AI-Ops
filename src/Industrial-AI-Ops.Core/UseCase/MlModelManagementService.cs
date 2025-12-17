using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Ports;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.Extensions.Logging;

namespace Industrial_AI_Ops.Core.UseCase;

public class MlModelManagementService : IMlModelManagementService
{
    private readonly IAnomalyDetectionService _anomalyDetectionService;
    private readonly IMlModelTrainer _modelTrainer;
    private readonly ILogger<MlModelManagementService> _logger;

    public MlModelManagementService(IAnomalyDetectionService anomalyDetectionService, IMlModelTrainer modelTrainer, ILogger<MlModelManagementService> logger)
    {
        _anomalyDetectionService = anomalyDetectionService;
        _modelTrainer = modelTrainer;
        _logger = logger;
    }
    
    public MlModelStatusResponse GetModelsStatus()
    {
        var isLoaded = _anomalyDetectionService.AreModelsLoaded();
        var isValidated = _modelTrainer.ValidateModelsAsync();

        return new MlModelStatusResponse
        {
            ModelsLoaded = isLoaded,
            ModelsValidated = isValidated,
            PumpModel = _modelTrainer.GetPumpModel() != null,
            CompressorModel = _modelTrainer.GetCompressorModel() != null,
            TurbineModel = _modelTrainer.GetTurbineModel() != null,
            MaintenanceModel = _modelTrainer.GetMaintenanceModel() != null,
            Timestamp = DateTime.UtcNow
        };
    }

    public async Task InitializeModels()
    {
        try
        {
            await _modelTrainer.InitializeAllModelsAsync();
            
            _anomalyDetectionService.LoadModels(
                _modelTrainer.GetPumpModel()!,
                _modelTrainer.GetCompressorModel()!,
                _modelTrainer.GetTurbineModel()!,
                _modelTrainer.GetMaintenanceModel()
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
            await _modelTrainer.RetrainAllModelsAsync();
            
            _anomalyDetectionService.LoadModels(
                _modelTrainer.GetPumpModel()!,
                _modelTrainer.GetCompressorModel()!,
                _modelTrainer.GetTurbineModel()!,
                _modelTrainer.GetMaintenanceModel()
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrain models");
            throw;
        }
    }
}