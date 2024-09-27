using Application.Models;
using Domain;
using MediatR;

namespace Application.Queries;

public class GetDocumentPrintJobQuery : IRequest<GetDocumentPrintJobResponse>
{
    public Guid Id { get; set; }
    public PrintJobStatus PrintJobStatus { get; set; }
}