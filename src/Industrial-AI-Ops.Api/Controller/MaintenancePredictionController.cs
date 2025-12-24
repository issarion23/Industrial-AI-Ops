using Industrial_AI_Ops.Api.Common;
using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.Enums;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Controller;

/// <summary>
/// 
/// </summary>
[Route("api/maintenance-prediction")]
[ApiVersion("1.0")]
public class MaintenancePredictionController : BaseController
{
    private readonly IMaintenancePredictionService _service;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
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

        return data.ToActionResult();
    }
    
    /// <summary>
    /// Create maintenance prediction
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MaintenancePrediction), StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateMaintenancePrediction(
        [FromBody] MaintenancePredictionDto prediction)
    {
        var result = await _service.CreateMaintenancePrediction(prediction);

        return result.ToActionResult();
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

        return result.ToActionResult();
    }
    
    /// <summary>
    /// Acknowledge maintenance prediction
    /// </summary>
    [HttpPut("{id}/acknowledge")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AcknowledgePrediction(string id)
    {
        var result = await _service.AcknowledgePrediction(id);

        if (result.IsFailure)
            return BadRequest();

        return Ok(new { message = "Prediction acknowledged", predictionId = id });
    }
}