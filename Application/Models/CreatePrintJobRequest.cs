namespace Application.Models;

public class CreatePrintJobRequest
{
    public Guid DocumentId { get; set; }
    public byte Priority { get; set; }
}
