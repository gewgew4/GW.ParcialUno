using Application.Exceptions;
using Domain;
using Infrastructure.Repo.Interfaces;
using MediatR;

namespace Application.Queries;

public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, Document>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDocumentQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Document> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
    {
        var document = await _unitOfWork.DocumentRepo.GetById(request.Id);
        if (document == null)
        {
            throw new NotFoundException($"Document with ID {request.Id} not found.");
        }

        return document;
    }
}