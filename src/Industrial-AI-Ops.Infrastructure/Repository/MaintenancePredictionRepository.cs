using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.Enums;
using Industrial_AI_Ops.Core.Ports.Repository;
using Industrial_AI_Ops.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Industrial_AI_Ops.Infrastructure.Repository;

public class MaintenancePredictionRepository : IMaintenancePredictionRepository
{
    private readonly AppDbContext _dbContext;

    public MaintenancePredictionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<MaintenancePrediction>> GetMaintenancePredictionAsync(
        string? equipmentId = null,
        RiskLevel? riskLevel = null,
        bool? acknowledged = null)
    {
        var query = _dbContext.MaintenancePredictions
            .Include(m => m.Equipment)
            .AsQueryable();

        if (!string.IsNullOrEmpty(equipmentId))
            query = query.Where(m => m.EquipmentId == equipmentId);

        if (riskLevel.HasValue)
            query = query.Where(m => m.RiskLevel == riskLevel.Value);

        if (acknowledged.HasValue)
            query = query.Where(m => m.IsAcknowledged == acknowledged.Value);

        return await query
            .OrderByDescending(m => m.RiskLevel)
            .ThenBy(m => m.PredictedFailureDate)
            .ToListAsync();
    }

    public async Task AddMaintenancePrediction(MaintenancePrediction prediction)
    {
        prediction.PredictionDate = DateTime.UtcNow;
        _dbContext.MaintenancePredictions.Add(prediction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateMaintenancePredictionAcknowledge(string id)
    {
        var prediction = await _dbContext.MaintenancePredictions.FindAsync(id);
        if (prediction == null)
            throw new InvalidOperationException($"Prediction with ID {id} not found");

        prediction.IsAcknowledged = true;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> GetMaintenancePredictionCountByRiskLevel(RiskLevel riskLevel)
    {
        return await _dbContext.MaintenancePredictions
            .CountAsync(m => m.RiskLevel == RiskLevel.Critical && !m.IsAcknowledged);
    }
}