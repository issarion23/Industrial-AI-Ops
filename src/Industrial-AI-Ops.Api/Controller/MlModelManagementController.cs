using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Controller;

[ApiController]
[Route("api/v{version:apiVersion}/ml-models")]
[Produces("application/json")]
[ApiVersion("1.0")]
public class MlModelManagementController : ControllerBase
{
    private readonly IMlModelManagementService _service;

    public MlModelManagementController(IMlModelManagementService service)
    {
        _service = service;
    }
    
    /// <summary>
    /// Get ML models status
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetModelsStatus()
    {
        var result = _service.GetModelsStatus();

        return Ok(result);
    }
    
    /// <summary>
    /// Initialize ML models
    /// </summary>
    [HttpPost("initialize")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> InitializeModels()
    {
        await _service.InitializeModels();

        return Ok();
    }
    
    /// <summary>
    /// Retrain all ML models
    /// </summary>
    [HttpPost("retrain")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RetrainModels()
    {
        await _service.RetrainModels();

        return Ok();
    }
}