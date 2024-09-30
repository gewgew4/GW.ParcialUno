using Application.Models;
using Infrastructure.Repo.Interfaces;
using MediatR;

namespace Application.Queries;
public class GetDocumentsPrintJobQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetDocumentsPrintJobQuery, List<GetDocumentPrintJobResponse>>
{
    public async Task<List<GetDocumentPrintJobResponse>> Handle(GetDocumentsPrintJobQuery request, CancellationToken cancellationToken)
    {
        var result = new List<GetDocumentPrintJobResponse>();

        foreach (var item in request.Ids)
        {
            var document = await unitOfWork.DocumentRepo.GetById(item);

            var printJobs = await unitOfWork.PrintJobRepo
                .GetWhere(p => p.DocumentId == item && p.Status == request.PrintJobStatus);
            
            if (document is not null && printJobs.Any())
            {
                var resultSingle = new GetDocumentPrintJobResponse
                {
                    Document = document,
                    PrintJobs = printJobs.ToList()
                };

                result.Add(resultSingle);
            }
        }

        return result;
    }
}