namespace Domain;

public class PrintResult
{
    public Guid DocumentId { get; private set; }
    public DateTime PrintedAt { get; private set; }
    public DateTime RecordedAt { get; private set; }
    public PrintResult(Guid documentId, DateTime printedAt)
    {
        DocumentId = documentId;
        PrintedAt = printedAt;
        RecordedAt = DateTime.UtcNow;
    }
}