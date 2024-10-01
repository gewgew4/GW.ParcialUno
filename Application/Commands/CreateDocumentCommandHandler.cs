using Domain;
using Infrastructure.Repo.Interfaces;
using MediatR;

namespace Application.Commands;

public class CreateDocumentCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateDocumentCommand, Guid>
{
    public async Task<Guid> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = new Document(request.Name, request.Content);

        await unitOfWork.DocumentRepo.Add(document);
        await unitOfWork.SaveAsync();

        return document.Id;
    }
}
