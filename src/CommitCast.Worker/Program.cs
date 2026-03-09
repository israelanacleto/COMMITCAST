using CommitCast.Worker;
using CommitCast.Core.Data;
using CommitCast.Core.Repositories;
using CommitCast.Core.Services;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<CommitCastDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IRepositoryRepository, RepositoryRepository>();
builder.Services.AddScoped<GitService>();
builder.Services.AddScoped<AIContentService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CommitCastDbContext>();
    await dbContext.Database.MigrateAsync();
}

host.Run();
