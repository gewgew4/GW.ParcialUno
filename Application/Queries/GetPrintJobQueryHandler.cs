using Application.Exceptions;
using Domain;
using Infrastructure.Repo.Interfaces;
using MediatR;

namespace Application.Queries;

public class GetPrintJobQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPrintJobQuery, PrintJob>
{
    public async Task<PrintJob> Handle(GetPrintJobQuery request, CancellationToken cancellationToken)
    {
        var printJob = await unitOfWork.PrintJobRepo.GetById(request.Id);
        if (printJob == null)
        {
            throw new NotFoundException($"PrintJob with ID {request.Id} not found.");
        }

        return printJob;
    }
}