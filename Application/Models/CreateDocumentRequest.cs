namespace Application.Models;

public class CreateDocumentRequest
{
    public string Name { get; set; }
    /// <summary>
    /// Base64 encoded string
    /// </summary>
    public string Content { get; set; }
    public byte Priority { get; set; }
}