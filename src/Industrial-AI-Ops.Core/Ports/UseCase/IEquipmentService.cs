using CSharpFunctionalExtensions;
using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Models;

namespace Industrial_AI_Ops.Core.Ports.UseCase;

public interface IEquipmentService
{
    Task<Result<List<Equipment>>> GetAllEquipment();
    Task<Result<Equipment?>> GetEquipmentById(string id);
    Task<Result> CreateEquipment(EquipmentDto equipment);
    Task<Result<Equipment>> UpdateEquipment(EquipmentDto equipment);
    Task<Result> DeleteEquipment(string id);
}