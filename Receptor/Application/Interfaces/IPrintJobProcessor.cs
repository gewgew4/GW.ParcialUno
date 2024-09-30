using Domain;

namespace Receptor.Application.Interfaces;

public interface IPrintJobProcessor
{
    Task ProcessJob(PrintJob job);
}