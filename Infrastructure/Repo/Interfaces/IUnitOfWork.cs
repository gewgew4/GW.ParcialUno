namespace Infrastructure.Repo.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDocumentRepo DocumentRepo { get; }
    IPrintJobRepo PrintJobRepo { get; }
    Task<int> SaveAsync();
}
