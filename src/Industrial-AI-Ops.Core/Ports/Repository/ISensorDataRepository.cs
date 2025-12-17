using Industrial_AI_Ops.Core.Models;

namespace Industrial_AI_Ops.Core.Ports.Repository;

public interface ISensorDataRepository
{
    Task<List<PumpSensorData>> GetPumpSensorDataAsync(
        string? equipmentId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 1000);
    Task AddPumpSensorDataAsync(PumpSensorData data);
    Task<PumpSensorData> GetPumpSensorDataByIdAsync(string id);

    Task<List<CompressorSensorData>> GetCompressorSensorDataAsync(
        string? equipmentId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 1000);
    Task AddCompressorSensorDataAsync(CompressorSensorData data);
    Task<CompressorSensorData> GetCompressorSensorDataByIdAsync(string id);

    Task<List<TurbineSensorData>> GetTurbineSensorDataAsync(
        string? equipmentId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 1000);

    Task AddTurbineSensorDataAsync(TurbineSensorData data);
    Task<TurbineSensorData> GetTurbineSensorDataByIdAsync(string id);

    Task<List<PumpSensorData>> GetPumpDataForCalculateVibration(string equipmentId, int limit);
    Task<List<CompressorSensorData>> GetCompressorDataForCalculateVibration(string equipmentId, int limit);
    Task<List<TurbineSensorData>> GetTurbineDataForCalculateVibration(string equipmentId, int limit);

    Task<List<PumpSensorData>> GetPumpDataForTrainMlModel(int limit);
    Task<List<CompressorSensorData>> GetCompressorDataForTrainMlModel(int limit);
    Task<List<TurbineSensorData>> GetTurbineDataForTrainMlModel(int limit);
}