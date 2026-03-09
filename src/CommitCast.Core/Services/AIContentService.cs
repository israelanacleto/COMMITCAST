using Mscc.GenerativeAI;
using CommitCast.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CommitCast.Core.Services;

public class AIContentService
{
    private readonly ILogger<AIContentService> _logger;
    private readonly GoogleAI _googleAI;

    public AIContentService(IConfiguration configuration, ILogger<AIContentService> logger)
    {
        _logger = logger;
        var apiKey = configuration["Gemini:ApiKey"] ?? throw new InvalidOperationException("Gemini API Key not configured");
        _googleAI = new GoogleAI(apiKey);
    }

    public async Task<string> GenerateLinkedInPostAsync(Post post)
    {
        try
        {
            var prompt = BuildPrompt(post);
            
            // Usando Gemini 2.5 Flash - modelo mais recente e rápido
            var model = _googleAI.GenerativeModel(model: "gemini-2.5-flash");
            
            var response = await model.GenerateContent(prompt);
            
            var generatedContent = response?.Text ?? string.Empty;
            
            _logger.LogInformation("Generated LinkedIn post for commit {Hash}", post.CommitHash);
            
            return generatedContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate content for commit {Hash}", post.CommitHash);
            throw;
        }
    }

    private string BuildPrompt(Post post)
    {
        return $@"Você é um especialista em criar posts profissionais para LinkedIn voltados para desenvolvedores.

Crie um post engajador para LinkedIn baseado neste commit:

**Mensagem do Commit:** {post.CommitMessage}
**Autor:** {post.CommitAuthor}
**Data:** {post.CommitDate:dd/MM/yyyy}
**Repositório:** {post.RepoUrl}

**Diretrizes:**
- Escreva em português brasileiro
- Tom profissional mas acessível
- Máximo de 150 palavras
- Use 2-3 emojis relevantes
- Destaque o valor técnico ou impacto do commit
- Incentive interação (ex: ""O que vocês acham?"")
- NÃO use hashtags em excesso (máximo 3)
- Foque no aprendizado ou conquista técnica

Retorne APENAS o texto do post, sem aspas ou formatação extra.";
    }
}
