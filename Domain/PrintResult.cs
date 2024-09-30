namespace Domain;

public class PrintResult : BaseEntity<PrintResult>
{
    public Guid DocumentId { get; set; }
    public string DocumentName { get; set; }
    public DateTime PrintedAt { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
}