using Microsoft.ML;

namespace Industrial_AI_Ops.Core.Ports;

public interface IMlModelTrainer
{
    bool ValidateModelsAsync();
    
    Task InitializeAllModelsAsync();

    ITransformer? GetPumpModel();
    ITransformer? GetCompressorModel();
    ITransformer? GetTurbineModel();
    ITransformer? GetMaintenanceModel();

    Task RetrainAllModelsAsync();
}