using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Ports.Repository;
using Industrial_AI_Ops.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Industrial_AI_Ops.Infrastructure.Repository;

public class EquipmentRepository : IEquipmentRepository
{
    private readonly AppDbContext _dbContext;
    
    public EquipmentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Equipment>> GetAllEquipmentAsync()
    {
        var equipment = await _dbContext.Equipment
            .Include(e => e.PumpSensors.Take(1))
            .Include(e => e.CompressorSensors.Take(1))
            .Include(e => e.TurbineSensors.Take(1))
            .Include(e => e.Predictions.Take(1))
            .ToListAsync();

        return equipment;
    }
    
    public async Task<Equipment?> GetEquipmentById(string id)
    {
        var equipment = await _dbContext.Equipment
                .Include(e => e.PumpSensors.Take(1))
                .Include(e => e.CompressorSensors.Take(1))
                .Include(e => e.TurbineSensors.Take(1))
                .Include(e => e.Predictions.Take(1))
                .FirstOrDefaultAsync(e => e.Id == id);

        return equipment;
    }

    public async Task CreateEquipment(Equipment equipment)
    {
        _dbContext.Equipment.Add(equipment);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task<Equipment> UpdateEquipment(Equipment equipment)
    {
        var existing = await _dbContext.Equipment.FindAsync(equipment.Id);
        if (existing == null)
            throw new InvalidOperationException($"Equipment with ID {equipment.Id} not found");

        existing.Name = equipment.Name;
        existing.Type = equipment.Type;
        existing.Location = equipment.Location;
        existing.Status = equipment.Status;
        existing.LastMaintenanceDate = equipment.LastMaintenanceDate;
        existing.HealthScore = equipment.HealthScore;

        await _dbContext.SaveChangesAsync(CancellationToken.None);

        return existing;
    }

    public async Task RemoveEquipment(string id)
    {
        var equipment = await _dbContext.Equipment.FindAsync(id);
        if (equipment == null)
            throw new InvalidOperationException($"Equipment with ID {id} not found");

        _dbContext.Equipment.Remove(equipment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> GetEquipmentCountByStatus(EquipmentStatus? status = null)
    {
        int count;
        switch (status)
        {
            case EquipmentStatus.Operational:
                count = await _dbContext.Equipment.CountAsync(e => e.Status == EquipmentStatus.Operational);
                break;
            case EquipmentStatus.Warning:
                count = await _dbContext.Equipment.CountAsync(e => e.Status == EquipmentStatus.Warning);
                break;
            case EquipmentStatus.Critical:
                count = await _dbContext.Equipment.CountAsync(e => e.Status == EquipmentStatus.Critical);
                break;
            case EquipmentStatus.Offline:
                count = await _dbContext.Equipment.CountAsync(e => e.Status == EquipmentStatus.Offline);
                break;
            case null:
                count = await _dbContext.Equipment.CountAsync();
                break;
            default:
                throw new InvalidOperationException();
        }

        return count;
    }

    public async Task<double> GetEquipmentAverageHealthScore()
    {
        if (!await _dbContext.Equipment.AnyAsync())
            return 0;
        
       return await _dbContext.Equipment.AverageAsync(e => e.HealthScore);
    }
}