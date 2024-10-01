using Application.Exceptions;
using Domain;
using Infrastructure.Repo.Interfaces;
using MediatR;

namespace Application.Queries;

public class GetPrintJobsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPrintJobsQuery, List<PrintJob>>
{
    public async Task<List<PrintJob>> Handle(GetPrintJobsQuery request, CancellationToken cancellationToken)
    {
        var printJob = await unitOfWork.PrintJobRepo.GetAll();
        if (printJob == null)
        {
            throw new NotFoundException($"No PrintJobs were not found.");
        }

        return printJob;
    }
}