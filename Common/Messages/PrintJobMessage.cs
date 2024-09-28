﻿namespace Common.Messages;

public class PrintJobMessage
{
    public Guid JobId { get; set; }
    public byte[] Content { get; set; }
    public Guid DocumentId { get; set; }
    public byte Priority { get; set; }
}