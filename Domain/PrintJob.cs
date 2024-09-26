namespace Domain;

public class PrintJob : BaseEntity<PrintJob>
{
    public Guid Id { get; private set; }
    public Guid DocumentId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public PrintJobStatus Status { get; private set; }

    public PrintJob(Guid documentId)
    {
        Id = Guid.NewGuid();
        DocumentId = documentId;
        CreatedAt = DateTime.UtcNow;
        Status = PrintJobStatus.Queued;
    }

    public void Complete()
    {
        Status = PrintJobStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public void Fail()
    {
        Status = PrintJobStatus.Failed;
    }
}