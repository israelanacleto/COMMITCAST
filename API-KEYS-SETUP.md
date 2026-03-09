# Configuração Local - NÃO COMMITAR!

## Gemini API Key
Pegue sua chave em: https://aistudio.google.com/app/apikey

### Windows (PowerShell):
```powershell
$env:Gemini__ApiKey = "SUA_CHAVE_AQUI"
```

### Linux/Mac:
```bash
export Gemini__ApiKey="SUA_CHAVE_AQUI"
```

### Visual Studio / Rider:
Adicione em: Properties/launchSettings.json > environmentVariables

### Ou use User Secrets (recomendado para dev):
```bash
cd src/CommitCast.API
dotnet user-secrets set "Gemini:ApiKey" "SUA_CHAVE_AQUI"

cd ../CommitCast.Worker
dotnet user-secrets set "Gemini:ApiKey" "SUA_CHAVE_AQUI"
```

## Produção
Use variáveis de ambiente no servidor/container.
