using AccountStatements.Api.Swagger;
using AccountStatements.Application.Statements;
using AccountStatements.Application.Statements.Queries.GetCustomerStatements;
using AccountStatements.Application.Statements.Queries.GetStatement;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace AccountStatements.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId:guid}/statements")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Lists a customer statements, optionally filtered by a month range.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<StatementDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(200, typeof(StatementDtoListExample))]
    [SwaggerResponseExample(400, typeof(ValidationErrorExample))]
    [SwaggerResponseExample(404, typeof(NotFoundErrorExample))]
    public async Task<ActionResult<List<StatementDto>>> GetAll(
        Guid customerId,
        [FromQuery] int? fromYear,
        [FromQuery] int? fromMonth,
        [FromQuery] int? toYear,
        [FromQuery] int? toMonth,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetCustomerStatementsQuery(customerId, fromYear, fromMonth, toYear, toMonth), ct);
        return Ok(result);
    }

    /// <summary>Gets one statement by customer, year and month.</summary>
    [HttpGet("{year:int}/{month:int}")]
    [ProducesResponseType(typeof(StatementDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(200, typeof(StatementDtoExample))]
    [SwaggerResponseExample(400, typeof(ValidationErrorExample))]
    [SwaggerResponseExample(404, typeof(NotFoundErrorExample))]
    public async Task<ActionResult<StatementDto>> GetOne(
        Guid customerId, int year, int month, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetStatementQuery(customerId, year, month), ct);
        return Ok(result);
    }
}
