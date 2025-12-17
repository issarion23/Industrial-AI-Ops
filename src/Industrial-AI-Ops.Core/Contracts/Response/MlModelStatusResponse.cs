namespace Industrial_AI_Ops.Core.Contracts.Response;

public class MlModelStatusResponse
{
    public bool ModelsLoaded { get; set; }
    public bool ModelsValidated { get; set; }
    public bool PumpModel { get; set; }
    public bool CompressorModel { get; set; }
    public bool TurbineModel { get; set; }
    public bool MaintenanceModel { get; set; }
    public DateTime Timestamp { get; set; }
}