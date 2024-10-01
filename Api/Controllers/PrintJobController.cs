using Application.Commands;
using Application.Models;
using Application.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrintJobController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePrintJob([FromBody] CreatePrintJobRequest dto)
    {
        var command = new CreatePrintJobCommand
        {
            DocumentId = dto.DocumentId,
            Priority = dto.Priority
        };

        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PrintJob>> GetPrintJob(Guid id)
    {
        var query = new GetPrintJobQuery { Id = id };
        var result = await mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<PrintJob>>> GetPrintJobs()
    {
        var query = new GetPrintJobsQuery();
        var result = await mediator.Send(query);

        if (result == null || result.Count == 0)
        {
            return NotFound();
        }

        return Ok(result);
    }
}