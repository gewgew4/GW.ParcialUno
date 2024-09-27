using Domain;
using Domain.Enums;
using Infrastructure.Repo.Interfaces;
using MediatR;

namespace Application.Commands;

public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateDocumentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = new Document(request.Name, request.Content);

        await _unitOfWork.DocumentRepo.Add(document);
        await _unitOfWork.SaveAsync();

        return document.Id;
    }
}
