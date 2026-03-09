namespace CommitCast.Core.Models;

public class Post
{
    public int Id { get; set; }
    public required string RepoUrl { get; set; }
    public required string CommitHash { get; set; }
    public required string CommitMessage { get; set; }
    public required string CommitAuthor { get; set; }
    public DateTime CommitDate { get; set; }
    public string? GeneratedContent { get; set; }
    public PostStatus Status { get; set; } = PostStatus.Draft;
    public DateTime? ScheduledAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum PostStatus
{
    Draft,
    Scheduled,
    Published,
    Failed
}
