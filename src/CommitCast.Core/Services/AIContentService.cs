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
        return $@"Você é Israel Igarashi, desenvolvedor backend especializado em .NET e C#, criando um post BILÍNGUE para LinkedIn focado em atrair oportunidades internacionais.

**CONTEXTO DO COMMIT:**
Mensagem: {post.CommitMessage}
Autor: {post.CommitAuthor}
Data: {post.CommitDate:dd/MM/yyyy}
Repositório: {post.RepoUrl}

**SEU ESTILO DE ESCRITA (MANTENHA EXATAMENTE):**

📐 ESTRUTURA BILÍNGUE:
- PRIMEIRA PARTE: Inglês completo (140-160 palavras)
- Linha separadora: ""---------------------------------- PT-BR ----------------------------------""
- SEGUNDA PARTE: Português completo (tradução adaptada, não literal)
- Ambas as versões devem ter a mesma estrutura e impacto

✍️ FORMATO (para cada idioma):
- Título chamativo em negrito com emojis estratégicos
- Parágrafos curtos e objetivos
- Seções com títulos em negrito quando relevante (ex: ""𝗧𝗲𝗰𝗵 𝗦𝘁𝗮𝗰𝗸:"")
- Bullets com emojis técnicos: 🚀 💾 🔐 🧠 ⚙️ 🎯
- Pergunta engajadora ao final

🎭 TOM (CRÍTICO PARA BUSCA DE EMPREGO):
- Técnico mas acessível (demonstre expertise sem arrogância)
- Mostre IMPACTO e RESULTADOS, não só o que fez
- Use primeira pessoa (""I implemented"" / ""Implementei"")
- Destaque DECISÕES ARQUITETURAIS e PORQUÊ das escolhas
- Foco em: escalabilidade, performance, boas práticas, clean code

💡 CONTEÚDO TÉCNICO (MOSTRE SUAS SKILLS):
- Mencione padrões de projeto: Clean Architecture, CQRS, DDD, Repository Pattern
- Tecnologias específicas: .NET 8, C#, Entity Framework Core, SQL Server
- Conceitos avançados: Dependency Injection, Async/Await, SOLID principles
- Ferramentas modernas: Docker, Git, CI/CD, Microservices (se aplicável)
- Integrações: APIs REST, Gemini AI, OAuth, JWT

🔧 TECNOLOGIAS PARA MENCIONAR (quando relevante):
Backend: .NET 8, ASP.NET Core, C# 12, Entity Framework Core
Arquitetura: Clean Architecture, CQRS, MediatR, DDD, Microservices
Banco: SQL Server, MongoDB, Redis
Cloud: Azure, AWS (se aplicável)
DevOps: Docker, GitHub Actions, CI/CD
APIs: REST, GraphQL, gRPC
Segurança: JWT, OAuth 2.0, ASP.NET Core Identity
Resiliência: Polly, Hangfire, RabbitMQ

🎯 OBJETIVO (BUSCA DE EMPREGO):
- Demonstre que você RESOLVE PROBLEMAS, não só escreve código
- Mostre evolução técnica e aprendizado contínuo
- Destaque decisões que melhoraram performance/escalabilidade/segurança
- Posicione-se como desenvolvedor SÊNIOR que pensa em arquitetura

✍️ HASHTAGS (ao final, depois do PT-BR):
Escolha 7-8 hashtags MIX de português e inglês:
- Sempre incluir: #DotNet #CSharp #BackendDevelopment #SoftwareDevelopment
- Adicionar 3-4 específicas do commit: #CleanArchitecture #EntityFramework #API #Microservices #DevOps #CloudComputing

❌ EVITE:
- Tom corporativo ou genérico
- ""Estou feliz em compartilhar"" (seja mais direto e técnico)
- Muitos emojis (máximo 5 no total)
- Jargões sem explicação
- Falar do commit em si (fale do IMPACTO técnico)

📝 EXEMPLO DE ESTRUTURA:

🚀 𝗕𝘂𝗶𝗹𝘁 𝗮 𝗿𝗼𝗯𝘂𝘀𝘁 𝗺𝘂𝗹𝘁𝗶-𝘁𝗲𝗻𝗮𝗻𝘁 𝘀𝘆𝘀𝘁𝗲𝗺 𝘂𝘀𝗶𝗻𝗴 .𝗡𝗘𝗧 𝟴

When developing [contexto], I needed to ensure [problema]. My approach:

𝗔𝗿𝗰𝗵𝗶𝘁𝗲𝗰𝘁𝘂𝗿𝗲:
🔧 Clean Architecture for separation of concerns
💾 CQRS with MediatR for complex business logic
🔐 JWT authentication with ASP.NET Core Identity

This implementation improved [resultado] and enabled [impacto].

What's your approach to [pergunta técnica]?

---------------------------------- PT-BR ----------------------------------

🚀 𝗖𝗼𝗻𝘀𝘁𝗿𝘂í 𝘂𝗺 𝘀𝗶𝘀𝘁𝗲𝗺𝗮 𝗺𝘂𝗹𝘁𝗶-𝘁𝗲𝗻𝗮𝗻𝘁 𝗿𝗼𝗯𝘂𝘀𝘁𝗼 𝗰𝗼𝗺 .𝗡𝗘𝗧 𝟴

[mesma estrutura adaptada para português]

Retorne APENAS o texto do post bilíngue, sem aspas ou formatação extra.";
    }
}
