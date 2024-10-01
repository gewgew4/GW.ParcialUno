using Domain;
using MediatR;

namespace Application.Queries;

public class GetPrintJobsQuery : IRequest<List<PrintJob>>;
