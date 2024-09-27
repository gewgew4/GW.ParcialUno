using Domain;
using MediatR;

namespace Application.Queries;

public class GetDocumentQuery : IRequest<Document>
{
    public Guid Id { get; set; }
}