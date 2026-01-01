using Microsoft.ML;

namespace Industrial_AI_Ops.Core.Ports.ML;

public interface IMlModelTrainService
{
    bool ValidateModelsAsync();
    
    Task InitializeAllModelsAsync();

    ITransformer? GetPumpModel();
    ITransformer? GetCompressorModel();
    ITransformer? GetTurbineModel();
    ITransformer? GetMaintenanceModel();

    Task RetrainAllModelsAsync();
}