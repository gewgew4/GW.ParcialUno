using Domain.Enums;

namespace Domain;

public class PrintJob : BaseEntity<PrintJob>
{
    public Guid Id { get; private set; }
    public Guid DocumentId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    /// <summary>
    /// 1: lowest - 10: highest
    /// </summary>
    public byte Priority { get; private set; }
    public PrintJobStatus Status { get; private set; }

    public PrintJob(Guid documentId, byte priority)
    {
        if (priority < 1 || priority > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(priority), "Priority must be between 1 and 10.");
        }

        Id = Guid.NewGuid();
        DocumentId = documentId;
        CreatedAt = DateTime.UtcNow;
        Priority = priority;
        Status = PrintJobStatus.None;
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

    public void Queue()
    {
        Status = PrintJobStatus.Queued;
    }
}