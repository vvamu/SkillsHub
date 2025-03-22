using Microsoft.EntityFrameworkCore.Storage;
using SkillsHub.Persistence;

namespace SkillsHub.Application.Helpers;
public abstract class AbstractTransactionService
{
    protected virtual ApplicationDbContext _context { get; set; }

    protected AbstractTransactionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return _context.Database.CreateExecutionStrategy();
    }
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync(IDbContextTransaction transaction)
    {
        await transaction.CommitAsync();
    }

    public async Task RollbackAsync(IDbContextTransaction transaction)
    {
        await transaction.RollbackAsync();
    }

}

