using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Models.RAG;
using Industrial_AI_Ops.Core.Ports.RAG;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Controller;

/// <summary>
/// 
/// </summary>
[Route("api/assistant")]
[ApiVersion("1.0")]
public class AssistantController : BaseController
{
    private readonly IRagService _ragService;

    /// <summary>
    /// Constructor
    /// </summary>
    public AssistantController(IRagService ragService)
    {
        _ragService = ragService;
    }

    /// <summary>
    /// Ask a question to the AI assistant
    /// </summary>
    /// <param name="request">Question request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>AI-generated answer with sources</returns>
    [HttpPost("ask")]
    [ProducesResponseType(typeof(RagAnswer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RagAnswer>> Ask(
        [FromBody] RagQuestionRequest request,
        CancellationToken ct = default)
    {
        var answer = await _ragService.AskAsync(request.Question, ct);

        return Ok(answer);
    }
}