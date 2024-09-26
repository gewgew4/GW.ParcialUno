using Domain.Enums;

namespace Domain;

public class Document : BaseEntity<Document>
{
    public string Name { get; private set; }
    public byte[] Content { get; private set; }
    /// <summary>
    /// 1: lowest - 10: highest
    /// </summary>
    public byte Priority { get; private set; }
    public DocumentStatus Status { get; private set; }

    public Document(string name, byte[] content, byte priority)
    {
        if (priority < 1 || priority > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(priority), "Priority must be between 1 and 10.");
        }

        Id = Guid.NewGuid();
        Name = name;
        Content = content;
        Priority = priority;
        Status = DocumentStatus.Pending;
    }

    public void SetStatus(DocumentStatus newStatus)
    {
        Status = newStatus;
    }
}

