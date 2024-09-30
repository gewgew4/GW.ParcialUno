namespace Infrastructure.Repo.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDocumentRepo DocumentRepo { get; }
    IPrintJobRepo PrintJobRepo { get; }
    IPrintResultRepo PrintResultRepo { get; }
    Task<int> SaveAsync();
}
