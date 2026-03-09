using CommitCast.Core.Data;
using CommitCast.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CommitCast.Core.Repositories;

public interface IRepositoryRepository
{
    Task<MonitoredRepository> CreateAsync(MonitoredRepository repo);
    Task<MonitoredRepository?> GetByIdAsync(int id);
    Task<List<MonitoredRepository>> GetAllActiveAsync();
    Task<MonitoredRepository> UpdateAsync(MonitoredRepository repo);
    Task DeleteAsync(MonitoredRepository repo);
}

public class RepositoryRepository : IRepositoryRepository
{
    private readonly CommitCastDbContext _context;

    public RepositoryRepository(CommitCastDbContext context)
    {
        _context = context;
    }

    public async Task<MonitoredRepository> CreateAsync(MonitoredRepository repo)
    {
        _context.MonitoredRepositories.Add(repo);
        await _context.SaveChangesAsync();
        return repo;
    }

    public async Task<MonitoredRepository?> GetByIdAsync(int id)
    {
        return await _context.MonitoredRepositories.FindAsync(id);
    }

    public async Task<List<MonitoredRepository>> GetAllActiveAsync()
    {
        return await _context.MonitoredRepositories
            .Where(r => r.IsActive)
            .ToListAsync();
    }

    public async Task<MonitoredRepository> UpdateAsync(MonitoredRepository repo)
    {
        _context.MonitoredRepositories.Update(repo);
        await _context.SaveChangesAsync();
        return repo;
    }

    public async Task DeleteAsync(MonitoredRepository repo)
    {
        _context.MonitoredRepositories.Remove(repo);
        await _context.SaveChangesAsync();
    }
}
