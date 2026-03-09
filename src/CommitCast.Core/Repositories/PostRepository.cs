using CommitCast.Core.Data;
using CommitCast.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CommitCast.Core.Repositories;

public interface IPostRepository
{
    Task<Post> CreateAsync(Post post);
    Task<Post?> GetByIdAsync(int id);
    Task<List<Post>> GetAllAsync();
    Task<List<Post>> GetByStatusAsync(PostStatus status);
    Task<Post> UpdateAsync(Post post);
    Task<bool> CommitExistsAsync(string commitHash);
}

public class PostRepository : IPostRepository
{
    private readonly CommitCastDbContext _context;

    public PostRepository(CommitCastDbContext context)
    {
        _context = context;
    }

    public async Task<Post> CreateAsync(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        return await _context.Posts.FindAsync(id);
    }

    public async Task<List<Post>> GetAllAsync()
    {
        return await _context.Posts
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Post>> GetByStatusAsync(PostStatus status)
    {
        return await _context.Posts
            .Where(p => p.Status == status)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Post> UpdateAsync(Post post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task<bool> CommitExistsAsync(string commitHash)
    {
        return await _context.Posts.AnyAsync(p => p.CommitHash == commitHash);
    }
}
