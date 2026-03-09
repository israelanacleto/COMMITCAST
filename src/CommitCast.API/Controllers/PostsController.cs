using CommitCast.Core.Models;
using CommitCast.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CommitCast.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly ILogger<PostsController> _logger;

    public PostsController(IPostRepository postRepository, ILogger<PostsController> logger)
    {
        _postRepository = postRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Post>>> GetAll()
    {
        var posts = await _postRepository.GetAllAsync();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetById(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
            return NotFound();

        return Ok(post);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<List<Post>>> GetByStatus(PostStatus status)
    {
        var posts = await _postRepository.GetByStatusAsync(status);
        return Ok(posts);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Post>> Update(int id, [FromBody] UpdatePostDto dto)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
            return NotFound();

        if (!string.IsNullOrEmpty(dto.GeneratedContent))
            post.GeneratedContent = dto.GeneratedContent;

        if (dto.Status.HasValue)
            post.Status = dto.Status.Value;

        if (dto.ScheduledAt.HasValue)
            post.ScheduledAt = dto.ScheduledAt.Value;

        await _postRepository.UpdateAsync(post);
        return Ok(post);
    }
}

public record UpdatePostDto
{
    public string? GeneratedContent { get; init; }
    public PostStatus? Status { get; init; }
    public DateTime? ScheduledAt { get; init; }
}
