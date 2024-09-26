using Infrastructure.Repo;
using Infrastructure.Repo.Interfaces;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly PContext _context;
    public IDocumentRepo DocumentRepo { get; private set; }
    public IPrintJobRepo PrintJobRepo { get; private set; }

    public UnitOfWork(PContext context)
    {
        _context = context;
        DocumentRepo = new DocumentRepo(_context);
        PrintJobRepo = new PrintJobRepo(_context);
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose() => _context.Dispose();
}
