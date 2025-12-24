using CSharpFunctionalExtensions;
using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.ML.Results;

namespace Industrial_AI_Ops.Core.Ports.UseCase;

public interface ISensorDataService
{
    Task<Result<List<PumpSensorData>>> GetPumpSensorData(
        string? equipmentId,
        DateTime? startDate,
        DateTime? endDate,
        int limit);
    Task<Result> AddPumpSensorData(PumpSensorDataDto request);
    Task<Result<AnomalyResult>> DetectPumpAnomaly(string id);

    Task<Result<List<CompressorSensorData>>> GetCompressorSensorData(
        string? equipmentId,
        DateTime? startDate,
        DateTime? endDate,
        int limit);

    Task<Result> AddCompressorSensorData(CompressorSensorDataDto request);
    Task<Result<AnomalyResult>> DetectCompressorAnomaly(string id);

    Task<Result<List<TurbineSensorData>>> GetTurbineSensorData(
        string? equipmentId,
        DateTime? startDate,
        DateTime? endDate,
        int limit);

    Task<Result> AddTurbineSensorData(TurbineSensorDataDto request);
    Task<Result<AnomalyResult>> DetectTurbineAnomaly(string id);
}