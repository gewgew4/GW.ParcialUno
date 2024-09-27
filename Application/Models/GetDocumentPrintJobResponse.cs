using Domain;

namespace Application.Models;

public class GetDocumentPrintJobResponse
{
    public Document Document { get; set; }
    public List<PrintJob> PrintJobs { get; set; }
}
