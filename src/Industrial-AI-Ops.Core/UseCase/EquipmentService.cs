using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Ports.Repository;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Industrial_AI_Ops.Core.UseCase;

public class EquipmentService : IEquipmentService
{
    private readonly IEquipmentRepository _repo;
    private readonly ILogger<EquipmentService> _logger;

    public EquipmentService(
        ILogger<EquipmentService> logger, 
        IEquipmentRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<List<Equipment>> GetAllEquipment()
    {
        var equipment = await _repo.GetAllEquipmentAsync();
        
        return equipment;
    }

    public async Task<Equipment?> GetEquipmentById(string id)
    {
        var equipment = await _repo.GetEquipmentById(id);

        if (equipment == null)
        {
            _logger.LogError($"Equipment with ID {id} not found");
            throw new  InvalidOperationException($"Equipment with ID {id} not found");
        }
        
        return equipment;
    }

    public async Task CreateEquipment(EquipmentDto equipment)
    {
        try
        {
            await _repo.CreateEquipment(equipment.Adapt<Equipment>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Equipment> UpdateEquipment(EquipmentDto equipment)
    {
        try
        {
            return await _repo.UpdateEquipment(equipment.Adapt<Equipment>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task DeleteEquipment(string id)
    {
        try
        {
            await _repo.RemoveEquipment(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}