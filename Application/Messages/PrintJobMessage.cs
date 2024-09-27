namespace Application.Messages;
public class PrintJobMessage
{
    public Guid JobId { get; set; }
    public Guid DocumentId { get; set; }
    public byte Priority { get; set; }
}