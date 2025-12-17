using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Ports.Repository;
using Industrial_AI_Ops.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Industrial_AI_Ops.Infrastructure.Repository;

public class SensorDataRepository : ISensorDataRepository
{
    private readonly AppDbContext _dbContext;

    public SensorDataRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<PumpSensorData>> GetPumpSensorDataAsync(
        string? equipmentId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 1000)
    {
        var query = _dbContext.PumpSensorData
            .Include(p => p.Equipment)
            .AsQueryable();

        if (!string.IsNullOrEmpty(equipmentId))
            query = query.Where(p => p.EquipmentId == equipmentId);

        if (startDate.HasValue)
            query = query.Where(p => p.Timestamp >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(p => p.Timestamp <= endDate.Value);

        return await query
            .OrderByDescending(p => p.Timestamp)
            .Take(limit)
            .ToListAsync();
    }

    public async Task AddPumpSensorDataAsync(PumpSensorData data)
    {
        data.Timestamp = DateTime.UtcNow;
        _dbContext.PumpSensorData.Add(data);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<PumpSensorData> GetPumpSensorDataByIdAsync(string id)
    {
        var data = await _dbContext.PumpSensorData.FindAsync(id);
        if (data == null)
             throw new InvalidOperationException($"Pump sensor data with ID {id} not found");

        return data;
    }
    
    public async Task<List<CompressorSensorData>> GetCompressorSensorDataAsync(
        string? equipmentId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 1000)
    {
        var query = _dbContext.CompressorSensorData
            .Include(p => p.Equipment)
            .AsQueryable();

        if (!string.IsNullOrEmpty(equipmentId))
            query = query.Where(p => p.EquipmentId == equipmentId);

        if (startDate.HasValue)
            query = query.Where(p => p.Timestamp >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(p => p.Timestamp <= endDate.Value);

        return await query
            .OrderByDescending(p => p.Timestamp)
            .Take(limit)
            .ToListAsync();
    }
    
    public async Task AddCompressorSensorDataAsync(CompressorSensorData data)
    {
        data.Timestamp = DateTime.UtcNow;
        _dbContext.CompressorSensorData.Add(data);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CompressorSensorData> GetCompressorSensorDataByIdAsync(string id)
    {
        var data = await _dbContext.CompressorSensorData.FindAsync(id);
        if (data == null)
            throw new InvalidOperationException($"Compressor sensor data with ID {id} not found");

        return data;
    }
    
    public async Task<List<TurbineSensorData>> GetTurbineSensorDataAsync(
        string? equipmentId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 1000)
    {
        var query = _dbContext.TurbineSensorData
            .Include(p => p.Equipment)
            .AsQueryable();

        if (!string.IsNullOrEmpty(equipmentId))
            query = query.Where(p => p.EquipmentId == equipmentId);

        if (startDate.HasValue)
            query = query.Where(p => p.Timestamp >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(p => p.Timestamp <= endDate.Value);

        return await query
            .OrderByDescending(p => p.Timestamp)
            .Take(limit)
            .ToListAsync();
    }
    
    public async Task AddTurbineSensorDataAsync(TurbineSensorData data)
    {
        data.Timestamp = DateTime.UtcNow;
        _dbContext.TurbineSensorData.Add(data);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TurbineSensorData> GetTurbineSensorDataByIdAsync(string id)
    {
        var data = await _dbContext.TurbineSensorData.FindAsync(id);
        if (data == null)
            throw new InvalidOperationException($"Turbine sensor data with ID {id} not found");

        return data;
    }

    public async Task<List<PumpSensorData>> GetPumpDataForCalculateVibration(string equipmentId, int limit)
    {
        return await _dbContext.PumpSensorData
            .Where(p => p.EquipmentId == equipmentId)
            .OrderByDescending(p => p.Timestamp)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<CompressorSensorData>> GetCompressorDataForCalculateVibration(string equipmentId, int limit)
    {
        return await _dbContext.CompressorSensorData
            .Where(p => p.EquipmentId == equipmentId)
            .OrderByDescending(p => p.Timestamp)
            .Take(limit)
            .ToListAsync();
    }
    
    public async Task<List<TurbineSensorData>> GetTurbineDataForCalculateVibration(string equipmentId, int limit)
    {
        return await _dbContext.TurbineSensorData
            .Where(p => p.EquipmentId == equipmentId)
            .OrderByDescending(p => p.Timestamp)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<PumpSensorData>> GetPumpDataForTrainMlModel(int limit)
    {
        return await _dbContext.PumpSensorData
            .OrderByDescending(d => d.Timestamp)
            .Take(limit)
            .ToListAsync();
    }
    
    public async Task<List<CompressorSensorData>> GetCompressorDataForTrainMlModel(int limit)
    {
        return await _dbContext.CompressorSensorData
            .OrderByDescending(d => d.Timestamp)
            .Take(limit)
            .ToListAsync();
    }
    
    public async Task<List<TurbineSensorData>> GetTurbineDataForTrainMlModel(int limit)
    {
        return await _dbContext.TurbineSensorData
            .OrderByDescending(d => d.Timestamp)
            .Take(limit)
            .ToListAsync();
    }
}