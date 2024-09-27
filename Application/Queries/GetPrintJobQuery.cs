using Domain;
using MediatR;

namespace Application.Queries;

public class GetPrintJobQuery : IRequest<PrintJob>
{
    public Guid Id { get; set; }
}