namespace CommitCast.Core.Models;

public class CommitInfo
{
    public required string Sha { get; set; }
    public required string Message { get; set; }
    public required string Author { get; set; }
    public DateTime Date { get; set; }
}
