using CommitCast.Core.Models;
using CommitCast.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CommitCast.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepositoriesController : ControllerBase
{
    private readonly IRepositoryRepository _repoRepository;
    private readonly ILogger<RepositoriesController> _logger;

    public RepositoriesController(IRepositoryRepository repoRepository, ILogger<RepositoriesController> logger)
    {
        _repoRepository = repoRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<MonitoredRepository>>> GetAll()
    {
        var repos = await _repoRepository.GetAllActiveAsync();
        return Ok(repos);
    }

    [HttpPost]
    public async Task<ActionResult<MonitoredRepository>> Create([FromBody] CreateRepositoryDto dto)
    {
        var repo = new MonitoredRepository
        {
            RepoUrl = dto.RepoUrl,
            GitHubToken = dto.GitHubToken
        };

        await _repoRepository.CreateAsync(repo);
        return CreatedAtAction(nameof(GetById), new { id = repo.Id }, repo);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MonitoredRepository>> GetById(int id)
    {
        var repo = await _repoRepository.GetByIdAsync(id);
        if (repo == null)
            return NotFound();

        return Ok(repo);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var repo = await _repoRepository.GetByIdAsync(id);
        if (repo == null)
            return NotFound();

        await _repoRepository.DeleteAsync(repo);
        return NoContent();
    }
}

public record CreateRepositoryDto
{
    public required string RepoUrl { get; init; }
    public string? GitHubToken { get; init; }
}
