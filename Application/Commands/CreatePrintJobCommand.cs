using MediatR;

namespace Application.Commands;

public class CreatePrintJobCommand : IRequest<Guid>
{
    public Guid DocumentId { get; set; }
}