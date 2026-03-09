# Script para testar Gemini API e listar modelos disponíveis
$apiKey = Read-Host "gen-lang-client-0472099101"

Write-Host "Listando modelos disponíveis..." -ForegroundColor Cyan

$response = Invoke-RestMethod -Uri "https://generativelanguage.googleapis.com/v1beta/models?key=$apiKey" -Method Get

Write-Host "`nModelos disponíveis:" -ForegroundColor Green
foreach ($model in $response.models) {
    Write-Host "  - $($model.name)" -ForegroundColor Yellow
    Write-Host "    Suporta: $($model.supportedGenerationMethods -join ', ')" -ForegroundColor Gray
}

Write-Host "`nTestando geração de conteúdo com primeiro modelo disponível..." -ForegroundColor Cyan
$firstModel = $response.models | Where-Object { $_.supportedGenerationMethods -contains "generateContent" } | Select-Object -First 1

if ($firstModel) {
    Write-Host "Usando modelo: $($firstModel.name)" -ForegroundColor Green
    
    $body = @{
        contents = @(
            @{
                parts = @(
                    @{
                        text = "Olá! Me diga apenas 'OK' se você está funcionando."
                    }
                )
            }
        )
    } | ConvertTo-Json -Depth 10
    
    $testResponse = Invoke-RestMethod `
        -Uri "https://generativelanguage.googleapis.com/v1beta/$($firstModel.name):generateContent?key=$apiKey" `
        -Method Post `
        -ContentType "application/json" `
        -Body $body
    
    Write-Host "`nResposta da IA:" -ForegroundColor Green
    Write-Host $testResponse.candidates[0].content.parts[0].text -ForegroundColor White
} else {
    Write-Host "Nenhum modelo com suporte a generateContent encontrado!" -ForegroundColor Red
}
