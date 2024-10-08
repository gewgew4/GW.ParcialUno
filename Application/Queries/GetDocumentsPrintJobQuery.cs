﻿using Application.Models;
using Domain.Enums;
using MediatR;

namespace Application.Queries;

public class GetDocumentsPrintJobQuery : IRequest<List<GetDocumentPrintJobResponse>>
{
    public Guid[] Ids { get; set; }
    public PrintJobStatus PrintJobStatus { get; set; }
}
