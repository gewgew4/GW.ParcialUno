using Application.Commands;
using Application.Models;
using Application.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateDocument([FromBody] CreateDocumentRequest dto)
    {
        var command = new CreateDocumentCommand
        {
            Name = dto.Name,
            Content = Convert.FromBase64String(dto.Content),
            Priority = dto.Priority
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Document>> GetDocument(Guid id)
    {
        var query = new GetDocumentQuery { Id = id };
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
