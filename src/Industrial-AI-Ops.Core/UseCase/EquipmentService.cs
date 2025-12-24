using CSharpFunctionalExtensions;
using Industrial_AI_Ops.Core.Common.Result;
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

    public async Task<Result<List<Equipment>>> GetAllEquipment()
    {
        try
        {
            return ResultFactory.Success(await _repo.GetAllEquipmentAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ResultFactory.Failure<List<Equipment>>(ErrorCode.NotFound, ex.Message);
        }
    }

    public async Task<Result<Equipment?>> GetEquipmentById(string id)
    {
        try
        {
            var equipment = await _repo.GetEquipmentById(id);

            if (equipment == null)
            {
                _logger.LogError($"Equipment with ID {id} not found");
                ResultFactory.Failure<Equipment?>(ErrorCode.NotFound, $"Equipment with ID {id} not found");
            }
        
            return ResultFactory.Success(equipment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ResultFactory.Failure<Equipment?>(ErrorCode.NotFound, ex.Message);
        }
    }

    public async Task<Result> CreateEquipment(EquipmentDto equipment)
    {
        try
        {
            await _repo.CreateEquipment(equipment.Adapt<Equipment>());
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure(ErrorCode.Validation, ex.Message);
        }
    }

    public async Task<Result<Equipment>> UpdateEquipment(EquipmentDto equipment)
    {
        try
        {
            return ResultFactory.Success(await _repo.UpdateEquipment(equipment.Adapt<Equipment>()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure<Equipment>(ErrorCode.Validation, ex.Message);
        }
    }

    public async Task<Result> DeleteEquipment(string id)
    {
        try
        {
            await _repo.RemoveEquipment(id);
            return ResultFactory.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return ResultFactory.Failure(ErrorCode.NotFound, ex.Message);
        }
    }
}