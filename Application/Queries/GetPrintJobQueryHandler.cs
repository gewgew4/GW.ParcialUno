using Application.Exceptions;
using Domain;
using Infrastructure.Repo.Interfaces;
using MediatR;

namespace Application.Queries;

public class GetPrintJobQueryHandler : IRequestHandler<GetPrintJobQuery, PrintJob>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPrintJobQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PrintJob> Handle(GetPrintJobQuery request, CancellationToken cancellationToken)
    {
        var printJob = await _unitOfWork.PrintJobRepo.GetById(request.Id);
        if (printJob == null)
        {
            throw new NotFoundException($"PrintJob with ID {request.Id} not found.");
        }

        return printJob;
    }
}