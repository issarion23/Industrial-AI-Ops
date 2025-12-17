using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Models;

namespace Industrial_AI_Ops.Core.Ports.UseCase;

public interface IEquipmentService
{
    Task<List<Equipment>> GetAllEquipment();
    Task<Equipment?> GetEquipmentById(string id);
    Task CreateEquipment(EquipmentDto equipment);
    Task<Equipment> UpdateEquipment(EquipmentDto equipment);
    Task DeleteEquipment(string id);
}