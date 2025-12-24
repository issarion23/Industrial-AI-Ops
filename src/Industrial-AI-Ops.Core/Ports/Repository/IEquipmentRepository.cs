using Industrial_AI_Ops.Core.Models;

namespace Industrial_AI_Ops.Core.Ports.Repository;

public interface IEquipmentRepository
{
    Task<List<Equipment>> GetAllEquipmentAsync();
    Task<Equipment?> GetEquipmentById(string id);
    Task CreateEquipment(Equipment equipment);
    Task<Equipment> UpdateEquipment(Equipment equipment);
    Task RemoveEquipment(string id);
    Task<int> GetEquipmentCountByStatus(EquipmentStatus? status = null);
    ValueTask<double> GetEquipmentAverageHealthScore();
}