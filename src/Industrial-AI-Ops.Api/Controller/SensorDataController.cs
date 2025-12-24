using Industrial_AI_Ops.Api.Common;
using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.ML.Results;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Controller;

/// <summary>
/// 
/// </summary>
[Route("api/sensor-data")]
[ApiVersion("1.0")]
public class SensorDataController : BaseController
{
    private readonly ISensorDataService _service;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public SensorDataController(ISensorDataService service)
    {
        _service = service;
    }
    
    /// <summary>
    /// Get pump sensor data with optional filtering
    /// </summary>
    [HttpGet("pump")]
    [ProducesResponseType(typeof(List<PumpSensorData>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PumpSensorData>>> GetPumpSensorData(
        string? equipmentId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 1000)
    {
        var data = await _service.GetPumpSensorData(equipmentId, startDate, endDate, limit);

        return data.ToActionResult();
    }
    
    /// <summary>
    /// Add pump sensor data
    /// </summary>
    [HttpPost("pump")]
    [ProducesResponseType(typeof(PumpSensorData), StatusCodes.Status201Created)]
    public async Task<ActionResult<PumpSensorData>> AddPumpSensorData([FromBody] PumpSensorDataDto request)
    {
        var result = await _service.AddPumpSensorData(request);

        return result.ToActionResult();
    }
    
    /// <summary>
    /// Detect anomaly in pump sensor data
    /// </summary>
    [HttpPost("pump/{id}/detect-anomaly")]
    [ProducesResponseType(typeof(AnomalyResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DetectPumpAnomaly(string id)
    {
        var result = await _service.DetectPumpAnomaly(id);

        return result.ToActionResult();
    }
    
    /// <summary>
    /// Get compressor sensor data with optional filtering
    /// </summary>
    [HttpGet("compressor")]
    [ProducesResponseType(typeof(List<CompressorSensorData>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CompressorSensorData>>> GetCompressorSensorData(
        string? equipmentId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 1000)
    {
        var data = await _service.GetCompressorSensorData(equipmentId, startDate, endDate, limit);

        return data.ToActionResult();
    }
    
    /// <summary>
    /// Add compressor sensor data
    /// </summary>
    [HttpPost("compressor")]
    [ProducesResponseType(typeof(CompressorSensorData), StatusCodes.Status201Created)]
    public async Task<ActionResult<CompressorSensorData>> AddCompressorSensorData([FromBody] CompressorSensorDataDto request)
    {
        var result = await _service.AddCompressorSensorData(request);

        return result.ToActionResult();
    }
    
    /// <summary>
    /// Detect anomaly in compressor sensor data
    /// </summary>
    [HttpPost("compressor/{id}/detect-anomaly")]
    [ProducesResponseType(typeof(AnomalyResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DetectCompressorAnomaly(string id)
    {
        var result = await _service.DetectCompressorAnomaly(id);

        return result.ToActionResult();
    }
    
    /// <summary>
    /// Get turbine sensor data with optional filtering
    /// </summary>
    [HttpGet("turbine")]
    [ProducesResponseType(typeof(List<TurbineSensorData>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TurbineSensorData>>> GetTurbineSensorData(
        string? equipmentId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 1000)
    {
        var data = await _service.GetTurbineSensorData(equipmentId, startDate, endDate, limit);

        return data.ToActionResult();
    }
    
    /// <summary>
    /// Add compressor sensor data
    /// </summary>
    [HttpPost("turbine")]
    [ProducesResponseType(typeof(TurbineSensorData), StatusCodes.Status201Created)]
    public async Task<ActionResult<TurbineSensorData>> AddTurbineSensorData([FromBody] TurbineSensorDataDto request)
    {
        var result = await _service.AddTurbineSensorData(request);

        return result.ToActionResult();
    }
    
    /// <summary>
    /// Detect anomaly in compressor sensor data
    /// </summary>
    [HttpPost("turbine/{id}/detect-anomaly")]
    [ProducesResponseType(typeof(AnomalyResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DetectTurbineAnomaly(string id)
    {
        var result = await _service.DetectTurbineAnomaly(id);

        return result.ToActionResult();
    }
}