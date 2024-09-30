namespace Receptor.Infrastructure.Interfaces;

public interface IDatabaseService
{
    Task UpdatePrintJobStatus(Guid jobId, int status);
}