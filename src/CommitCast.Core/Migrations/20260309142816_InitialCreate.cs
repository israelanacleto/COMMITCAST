using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommitCast.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonitoredRepositories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LocalPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastCommitHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastCheckedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoredRepositories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CommitHash = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CommitMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CommitAuthor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CommitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GeneratedContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonitoredRepositories_RepoUrl",
                table: "MonitoredRepositories",
                column: "RepoUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CommitHash",
                table: "Posts",
                column: "CommitHash");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Status",
                table: "Posts",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitoredRepositories");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
