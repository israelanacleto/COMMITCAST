using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommitCast.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddGitHubTokenToRepository : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GitHubToken",
                table: "MonitoredRepositories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitHubToken",
                table: "MonitoredRepositories");
        }
    }
}
