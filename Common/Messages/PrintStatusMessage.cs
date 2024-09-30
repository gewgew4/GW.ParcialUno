namespace Common.Messages;

public class PrintStatusMessage
{
    public string OK { get; set; } = "OK";
    public DateTime PrintDate { get; set; } = DateTime.MinValue;
    public string DocumentName { get; set; } = string.Empty;
}
