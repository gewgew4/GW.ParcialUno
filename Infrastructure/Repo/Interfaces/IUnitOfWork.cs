namespace Infrastructure.Repo.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDocumentRepo Documents { get; }
    IPrintJobRepo PrintJobs { get; }
    Task<int> SaveAsync();
}
