using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.ML;
using Industrial_AI_Ops.Core.Models.ML.Results;
using Microsoft.ML;

namespace Industrial_AI_Ops.Core.Ports.ML;

public interface IAnomalyDetectionService
{
    Task<AnomalyResult> DetectPumpAnomalyAsync(PumpSensorData data);
    Task<AnomalyResult> DetectCompressorAnomalyAsync(CompressorSensorData data);
    Task<AnomalyResult> DetectTurbineAnomalyAsync(TurbineSensorData data);
    Task<MaintenancePredictionResult> PredictMaintenanceAsync(MaintenanceInput input);
    bool AreModelsLoaded();

    void LoadModels(
        ITransformer pumpModel,
        ITransformer compressorModel,
        ITransformer turbineModel,
        ITransformer? maintenanceModel = null);
}