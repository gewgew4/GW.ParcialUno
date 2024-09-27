using Domain.Enums;

namespace Domain;

public class Document : BaseEntity<Document>
{
    public string Name { get; private set; }
    public byte[] Content { get; private set; }
    public DocumentStatus Status { get; private set; }

    public Document(string name, byte[] content)
    {
        Id = Guid.NewGuid();
        Name = name;
        Content = content;
        Status = DocumentStatus.Pending;
    }

    public void SetStatus(DocumentStatus newStatus)
    {
        Status = newStatus;
    }
}

