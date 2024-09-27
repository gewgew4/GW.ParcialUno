using Application.Exceptions;
using Application.Models;
using Infrastructure.Repo.Interfaces;
using MediatR;

namespace Application.Queries;

public class GetDocumentPrintJobQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetDocumentPrintJobQuery, GetDocumentPrintJobResponse>
{
    public async Task<GetDocumentPrintJobResponse> Handle(GetDocumentPrintJobQuery request, CancellationToken cancellationToken)
    {
        var document = await unitOfWork.DocumentRepo.GetById(request.Id);
        if (document == null)
        {
            throw new NotFoundException($"Document with ID {request.Id} not found.");
        }

        var printJobs = await unitOfWork.PrintJobRepo
            .GetWhere(p => p.DocumentId == request.Id && p.Status == request.PrintJobStatus);

        if (!printJobs.Any())
        {
            throw new NotFoundException($"Document with ID {request.Id} with no print jobs.");
        }

        var result = new GetDocumentPrintJobResponse
        {
            Document = document,
            PrintJobs = printJobs.ToList()
        };

        return result;
    }
}
