using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.ML.Results;

namespace Industrial_AI_Ops.Core.Ports.UseCase;

public interface ISensorDataService
{
    Task<List<PumpSensorData>> GetPumpSensorData(
        string? equipmentId,
        DateTime? startDate,
        DateTime? endDate,
        int limit);
    Task AddPumpSensorData(PumpSensorDataDto request);
    Task<AnomalyResult> DetectPumpAnomaly(string id);

    Task<List<CompressorSensorData>> GetCompressorSensorData(
        string? equipmentId,
        DateTime? startDate,
        DateTime? endDate,
        int limit);

    Task AddCompressorSensorData(CompressorSensorDataDto request);
    Task<AnomalyResult> DetectCompressorAnomaly(string id);

    Task<List<TurbineSensorData>> GetTurbineSensorData(
        string? equipmentId,
        DateTime? startDate,
        DateTime? endDate,
        int limit);

    Task AddTurbineSensorData(TurbineSensorDataDto request);
    Task<AnomalyResult> DetectTurbineAnomaly(string id);
}