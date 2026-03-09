# CommitCast

Transforme seus commits em posts profissionais no LinkedIn automaticamente usando IA.

## 🚀 Sprint 1 - Setup + Captura de Commits

### Status: ✅ Completo

A Sprint 1 implementa a captura automática de commits de repositórios Git e armazenamento no banco de dados.

### Funcionalidades Implementadas

- ✅ Worker Service que monitora repositórios Git
- ✅ Captura de commits usando LibGit2Sharp
- ✅ Armazenamento de posts em status "Draft"
- ✅ API REST para gerenciar posts e repositórios
- ✅ Entity Framework Core com SQL Server
- ✅ Migrations automáticas

### Estrutura do Projeto

```
CommitCast/
├── src/
│   ├── CommitCast.API/          # API REST
│   │   └── Controllers/
│   ├── CommitCast.Worker/       # Background Service
│   └── CommitCast.Core/         # Camada de domínio
│       ├── Models/              # Entidades
│       ├── Data/                # DbContext
│       ├── Repositories/        # Repositórios
│       └── Services/            # Serviços
```

### 📋 Pré-requisitos

- .NET 10.0 SDK
- SQL Server 2019+ ou SQL Server Express
- Git instalado

### 🔧 Configuração

1. **Clone o repositório**
```bash
git clone <seu-repositorio>
cd CommitCast
```

2. **Configure o SQL Server**

Atualize a connection string em:
- `src/CommitCast.API/appsettings.json`
- `src/CommitCast.Worker/appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=CommitCastDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

3. **Execute as migrations**
```bash
cd src/CommitCast.API
dotnet ef database update
```

### 🏃 Executando

**API:**
```bash
cd src/CommitCast.API
dotnet run
```
A API estará disponível em `https://localhost:5001`

**Worker:**
```bash
cd src/CommitCast.Worker
dotnet run
```

### 📝 Endpoints da API

#### Repositórios
- `GET /api/repositories` - Lista todos os repositórios monitorados
- `POST /api/repositories` - Adiciona um novo repositório
- `GET /api/repositories/{id}` - Busca repositório por ID

**Exemplo de cadastro:**
```bash
curl -X POST https://localhost:5001/api/repositories \
  -H "Content-Type: application/json" \
  -d '{"repoUrl": "https://github.com/seu-usuario/seu-repo.git"}'
```

#### Posts
- `GET /api/posts` - Lista todos os posts
- `GET /api/posts/{id}` - Busca post por ID
- `GET /api/posts/status/Draft` - Lista posts em Draft
- `PUT /api/posts/{id}` - Atualiza um post

### ✅ Critério de Sucesso da Sprint 1

**"dotnet run --project Worker salva posts Draft no banco"**

Para testar:

1. Execute o Worker
2. Cadastre um repositório via API
3. Aguarde 5 minutos (ou reinicie o Worker)
4. Verifique os posts criados: `GET /api/posts/status/Draft`

### 🗄️ Modelo de Dados

**Post**
- Id, RepoUrl, CommitHash, CommitMessage
- CommitAuthor, CommitDate, GeneratedContent
- Status (Draft, Scheduled, Published, Failed)
- ScheduledAt, PublishedAt, CreatedAt

**MonitoredRepository**
- Id, RepoUrl, LocalPath, LastCommitHash
- IsActive, CreatedAt, LastCheckedAt

### 🔄 Próximas Sprints

- **Sprint 2:** Geração de conteúdo com IA (Anthropic Claude)
- **Sprint 3:** Publicação no LinkedIn via OAuth
- **Sprint 4:** Multi-usuário + Monitoramento de mensagens
- **Sprint 5:** Templates personalizados + LGPD
- **Sprint 6:** Deploy no Azure com CI/CD

### 📄 Licença

MIT License - Israel, 2025
