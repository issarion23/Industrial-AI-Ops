using Industrial_AI_Ops.Core.Models.ML.Results;

namespace Industrial_AI_Ops.Core.Contracts.Response;

public class PredictMaintenanceResponse
{
    public string EquipmentId { get; set; }
    public string EquipmentName { get; set; }
    public double CurrentHealthScore { get; set; } 
    public MaintenancePredictionResult Prediction { get; set; }
    public DateTime Timestamp { get; set; }
}