using CommitCast.Core.Models;
using CommitCast.Core.Repositories;
using CommitCast.Core.Services;

namespace CommitCast.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CommitCast Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessRepositoriesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing repositories");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    private async Task ProcessRepositoriesAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var repoRepository = scope.ServiceProvider.GetRequiredService<IRepositoryRepository>();
        var postRepository = scope.ServiceProvider.GetRequiredService<IPostRepository>();
        var gitService = scope.ServiceProvider.GetRequiredService<GitService>();

        var repositories = await repoRepository.GetAllActiveAsync();
        _logger.LogInformation("Processing {Count} repositories", repositories.Count);

        foreach (var repo in repositories)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            try
            {
                var commits = await gitService.GetNewCommitsAsync(repo);

                foreach (var commit in commits)
                {
                    if (await postRepository.CommitExistsAsync(commit.Sha))
                    {
                        _logger.LogDebug("Commit {Hash} already exists, skipping", commit.Sha);
                        continue;
                    }

                    var post = new Post
                    {
                        RepoUrl = repo.RepoUrl,
                        CommitHash = commit.Sha,
                        CommitMessage = commit.Message,
                        CommitAuthor = commit.Author.Name,
                        CommitDate = commit.Author.When.UtcDateTime,
                        Status = PostStatus.Draft
                    };

                    await postRepository.CreateAsync(post);
                    _logger.LogInformation("Created draft post for commit {Hash}", commit.Sha);
                }

                repo.LastCommitHash = commits.FirstOrDefault()?.Sha ?? repo.LastCommitHash;
                repo.LastCheckedAt = DateTime.UtcNow;
                await repoRepository.UpdateAsync(repo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing repository {RepoUrl}", repo.RepoUrl);
            }
        }
    }
}
