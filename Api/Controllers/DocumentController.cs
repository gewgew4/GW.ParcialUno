using Application.Commands;
using Application.Models;
using Application.Queries;
using Domain;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateDocument([FromBody] CreateDocumentRequest dto)
    {
        var command = new CreateDocumentCommand
        {
            Name = dto.Name,
            Content = Convert.FromBase64String(dto.Content)
        };

        var result = await mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Document>> GetDocument(Guid id)
    {
        var query = new GetDocumentQuery { Id = id };
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}/printed")]
    public async Task<ActionResult<Document>> GetDocumentPrinted(Guid id)
    {
        var query = new GetDocumentPrintJobQuery { Id = id, PrintJobStatus = PrintJobStatus.Completed };
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("printStatus")]
    public async Task<ActionResult<Document>> GetDocumentsPrinted([FromQuery] Guid[] ids, PrintJobStatus status)
    {
        var query = new GetDocumentsPrintJobQuery { Ids = ids, PrintJobStatus = status };
        var result = await mediator.Send(query);

        return Ok(result);
    }
}
