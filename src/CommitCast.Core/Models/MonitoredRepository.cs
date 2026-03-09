namespace CommitCast.Core.Models;

public class MonitoredRepository
{
    public int Id { get; set; }
    public required string RepoUrl { get; set; }
    public string? LocalPath { get; set; }
    public string? LastCommitHash { get; set; }
    public string? GitHubToken { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastCheckedAt { get; set; }
}
