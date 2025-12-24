using Industrial_AI_Ops.Api.Common;
using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Controller;

/// <summary>
/// 
/// </summary>
[Route("api/ml-models")]
[ApiVersion("1.0")]
public class MlModelManagementController : BaseController
{
    private readonly IMlModelManagementService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public MlModelManagementController(IMlModelManagementService service)
    {
        _service = service;
    }
    
    /// <summary>
    /// Get ML models status
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(typeof(MlModelStatusResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<MlModelStatusResponse>> GetModelsStatus()
    {
        var result = _service.GetModelsStatus();

        return result.ToActionResult();
    }
    
    /// <summary>
    /// Initialize ML models
    /// </summary>
    [HttpPost("initialize")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> InitializeModels()
    {
        var result = await _service.InitializeModels();

        return result.ToActionResult();
    }
    
    /// <summary>
    /// Retrain all ML models
    /// </summary>
    [HttpPost("retrain")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RetrainModels()
    {
        var result = await _service.RetrainModels();

        return result.ToActionResult();
    }
}