using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Controller;

[ApiController]
[Route("api/v{version:apiVersion}/equipment")]
[Produces("application/json")]
[ApiVersion("1.0")]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService _equipmentService;

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
        
        return Ok(equipments);
    }

    /// <summary>
    /// Get equipment by id
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Equipment), StatusCodes.Status200OK)]
    public async Task<ActionResult<Equipment>> GetEquipmentById(string id)
    {
        var equipment = await _equipmentService.GetEquipmentById(id);

        return Ok(equipment);
    }
    
    /// <summary>
    /// Create equipment
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateEquipment([FromBody]EquipmentDto request)
    {
        await _equipmentService.CreateEquipment(request);
        
        return Ok();
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
        
        return Ok(existing);
    }
    
    /// <summary>
    /// Delete equipment
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Equipment), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteEquipment(string id)
    {
        await _equipmentService.DeleteEquipment(id);
        
        return NoContent();
    }
}