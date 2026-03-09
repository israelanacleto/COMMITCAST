using LibGit2Sharp;
using CommitCast.Core.Models;
using Microsoft.Extensions.Logging;

namespace CommitCast.Core.Services;

public class GitService
{
    private readonly ILogger<GitService> _logger;

    public GitService(ILogger<GitService> logger)
    {
        _logger = logger;
    }

    public async Task<List<Commit>> GetNewCommitsAsync(MonitoredRepository repo)
    {
        try
        {
            if (string.IsNullOrEmpty(repo.LocalPath) || !Directory.Exists(repo.LocalPath))
            {
                _logger.LogWarning("Local path not found for repo {RepoUrl}. Cloning...", repo.RepoUrl);
                await CloneRepositoryAsync(repo);
            }

            using var repository = new Repository(repo.LocalPath);
            
            // Pull latest changes
            PullLatestChanges(repository);

            var commits = new List<Commit>();
            var lastCommit = string.IsNullOrEmpty(repo.LastCommitHash) 
                ? null 
                : repository.Lookup<Commit>(repo.LastCommitHash);

            foreach (var commit in repository.Commits)
            {
                if (lastCommit != null && commit.Sha == lastCommit.Sha)
                    break;

                commits.Add(commit);
            }

            _logger.LogInformation("Found {Count} new commits for repo {RepoUrl}", commits.Count, repo.RepoUrl);
            return commits;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting commits for repo {RepoUrl}", repo.RepoUrl);
            throw;
        }
    }

    private async Task CloneRepositoryAsync(MonitoredRepository repo)
    {
        var tempPath = Path.Combine(Path.GetTempPath(), "CommitCast", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _logger.LogInformation("Cloning repository {RepoUrl} to {Path}", repo.RepoUrl, tempPath);
        
        await Task.Run(() =>
        {
            Repository.Clone(repo.RepoUrl, tempPath);
        });

        repo.LocalPath = tempPath;
    }

    private void PullLatestChanges(Repository repository)
    {
        try
        {
            var remote = repository.Network.Remotes["origin"];
            var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
            
            Commands.Fetch(repository, remote.Name, refSpecs, null, "Fetching");
            
            var trackedBranch = repository.Head.TrackedBranch;
            if (trackedBranch != null)
            {
                repository.Reset(ResetMode.Hard, trackedBranch.Tip);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not pull latest changes. Continuing with existing commits.");
        }
    }
}
