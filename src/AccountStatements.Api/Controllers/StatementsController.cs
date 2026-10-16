using AccountStatements.Api.Contracts;
using AccountStatements.Application.Statements.Commands.GenerateStatements;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountStatements.Api.Controllers;

[ApiController]
[Route("api/statements")]
public class StatementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StatementsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Generates and emails statements for all customers for the given month.</summary>
    [HttpPost("generate")]
    [ProducesResponseType(typeof(GenerateStatementsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenerateStatementsResult>> Generate(
        [FromBody] GenerateStatementsRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new GenerateStatementsCommand(request.Year, request.Month), ct);
        return Ok(result);
    }
}
