using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.Enums;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Controller;

[ApiController]
[Route("api/v{version:apiVersion}/maintenance-prediction")]
[Produces("application/json")]
[ApiVersion("1.0")]
public class MaintenancePredictionController : ControllerBase
{
    private readonly IMaintenancePredictionService _service;
    
    public MaintenancePredictionController(IMaintenancePredictionService service)
    {
        _service = service;
    }
    
    /// <summary>
    /// Get maintenance predictions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<MaintenancePrediction>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MaintenancePrediction>>> GetMaintenancePredictions(
        string? equipmentId = null,
        RiskLevel? riskLevel = null,
        bool? acknowledged = null)
    {
        var data = await _service.GetMaintenancePrediction(equipmentId, riskLevel, acknowledged);

        return Ok(data);
    }
    
    /// <summary>
    /// Create maintenance prediction
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MaintenancePrediction), StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateMaintenancePrediction(
        [FromBody] MaintenancePredictionDto prediction)
    {
        await _service.CreateMaintenancePrediction(prediction);

        return Ok();
    }
    
    /// <summary>
    /// Predict maintenance for equipment
    /// </summary>
    [HttpPost("{equipmentId}/predict-maintenance")]
    [ProducesResponseType(typeof(PredictMaintenanceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PredictMaintenance(string equipmentId)
    {
        var result = await _service.PredictMaintenance(equipmentId);

        return Ok(result);
    }
    
    /// <summary>
    /// Acknowledge maintenance prediction
    /// </summary>
    [HttpPut("{id}/acknowledge")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AcknowledgePrediction(string id)
    {
        await _service.AcknowledgePrediction(id);

        return Ok(new { message = "Prediction acknowledged", predictionId = id });
    }
}