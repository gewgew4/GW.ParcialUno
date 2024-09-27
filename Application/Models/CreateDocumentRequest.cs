namespace Application.Models;

public class CreateDocumentRequest
{
    public string Name { get; set; }
    // Base64 encoded string
    public string Content { get; set; }
    public byte Priority { get; set; }
}