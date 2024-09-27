using Application.Commands;
using Application.Models;
using Application.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrintJobController : ControllerBase
{
    private readonly IMediator _mediator;

    public PrintJobController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePrintJob([FromBody] CreatePrintJobRequest dto)
    {
        var command = new CreatePrintJobCommand
        {
            DocumentId = dto.DocumentId,
            Priority = dto.Priority
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PrintJob>> GetPrintJob(Guid id)
    {
        var query = new GetPrintJobQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}