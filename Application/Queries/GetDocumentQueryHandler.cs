using Application.Exceptions;
using Domain;
using Infrastructure.Repo.Interfaces;
using MediatR;

namespace Application.Queries;

public class GetDocumentQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetDocumentQuery, Document>
{
    public async Task<Document> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
    {
        var document = await unitOfWork.DocumentRepo.GetById(request.Id);
        if (document == null)
        {
            throw new NotFoundException($"Document with ID {request.Id} not found.");
        }

        return document;
    }
}