using MediatR;

namespace Application.Commands;

public class CreateDocumentCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public byte[] Content { get; set; }
}
