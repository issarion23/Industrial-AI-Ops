using Industrial_AI_Ops.Api.Common;
using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Controller;

/// <summary>
/// 
/// </summary>
[Route("api/equipment")]
[ApiVersion("1.0")]
public class EquipmentController : BaseController
{
    private readonly IEquipmentService _equipmentService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="equipmentService"></param>
    public EquipmentController(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    /// <summary>
    /// Get all equipment
    /// </summary>
    [HttpGet("list")]
    [ProducesResponseType(typeof(List<Equipment>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Equipment>>> GetAllEquipment()
    {
        var equipments = await _equipmentService.GetAllEquipment();

        return equipments.ToActionResult();
    }

    /// <summary>
    /// Get equipment by id
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Equipment), StatusCodes.Status200OK)]
    public async Task<ActionResult<Equipment>> GetEquipmentById(string id)
    {
        var equipment = await _equipmentService.GetEquipmentById(id);

        return equipment.ToActionResult();
    }
    
    /// <summary>
    /// Create equipment
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateEquipment([FromBody]EquipmentDto request)
    {
        var result = await _equipmentService.CreateEquipment(request);

        return result.ToActionResult();
    }
    
    /// <summary>
    /// Update equipment
    /// </summary>
    [HttpPut]
    [ProducesResponseType(typeof(Equipment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Equipment>> UpdateEquipment([FromBody] EquipmentDto request)
    {
        var existing = await _equipmentService.UpdateEquipment(request);
        
        return existing.ToActionResult();
    }
    
    /// <summary>
    /// Delete equipment
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Equipment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteEquipment(string id)
    {
        var result = await _equipmentService.DeleteEquipment(id);
        
        return result.ToActionResult();
    }
}