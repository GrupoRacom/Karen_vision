# Script de PowerShell para subir KarenVision a GitHub
# Automatiza la creación del repositorio y la subida del código

Write-Host "🚀 Script de GitHub para KarenVision" -ForegroundColor Green
Write-Host "======================================" -ForegroundColor Green
Write-Host ""

# Verificar que estamos en el directorio correcto
if (-not (Test-Path "KarenVision.csproj")) {
    Write-Host "❌ Error: No se encuentra KarenVision.csproj en el directorio actual" -ForegroundColor Red
    Write-Host "Por favor ejecuta este script desde el directorio del proyecto" -ForegroundColor Yellow
    exit 1
}

Write-Host "✅ Directorio del proyecto verificado" -ForegroundColor Green

# Verificar que Git esté inicializado
try {
    $gitStatus = git status 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Error: Git no está inicializado" -ForegroundColor Red
        exit 1
    }
    Write-Host "✅ Repositorio Git inicializado" -ForegroundColor Green
} catch {
    Write-Host "❌ Error: Git no está disponible" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "📋 Pasos para subir a GitHub:" -ForegroundColor Cyan
Write-Host ""
Write-Host "1️⃣  Ir a https://github.com/new" -ForegroundColor White
Write-Host "2️⃣  Crear repositorio con nombre: Karen_Vision" -ForegroundColor White  
Write-Host "3️⃣  NO inicializar con README (ya lo tenemos)" -ForegroundColor White
Write-Host "4️⃣  Copiar la URL del repositorio" -ForegroundColor White
Write-Host ""

# Solicitar URL del repositorio
do {
    $repoUrl = Read-Host "📝 Pega la URL de tu repositorio GitHub (ej: https://github.com/usuario/Karen_Vision.git)"
    if ([string]::IsNullOrWhiteSpace($repoUrl)) {
        Write-Host "⚠️  La URL no puede estar vacía" -ForegroundColor Yellow
    }
} while ([string]::IsNullOrWhiteSpace($repoUrl))

Write-Host ""
Write-Host "🔗 Configurando remote origin..." -ForegroundColor Cyan

try {
    # Agregar el remote origin
    git remote add origin $repoUrl
    Write-Host "✅ Remote origin configurado" -ForegroundColor Green
} catch {
    # Si ya existe, actualizarlo
    git remote set-url origin $repoUrl
    Write-Host "✅ Remote origin actualizado" -ForegroundColor Green
}

Write-Host ""
Write-Host "📤 Subiendo código a GitHub..." -ForegroundColor Cyan

try {
    # Hacer push al repositorio
    git push -u origin master
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Código subido exitosamente!" -ForegroundColor Green
    } else {
        Write-Host "❌ Error al subir el código" -ForegroundColor Red
        Write-Host "Intenta manualmente: git push -u origin master" -ForegroundColor Yellow
        exit 1
    }
} catch {
    Write-Host "❌ Error al subir el código" -ForegroundColor Red
    Write-Host "Intenta manualmente: git push -u origin master" -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "🎉 ¡ÉXITO! Tu proyecto KarenVision está en GitHub" -ForegroundColor Green
Write-Host ""
Write-Host "📊 Estadísticas del repositorio:" -ForegroundColor Cyan

# Mostrar estadísticas
$commitCount = git rev-list --count HEAD
$fileCount = (git ls-files | Measure-Object).Count

Write-Host "   📁 Archivos: $fileCount" -ForegroundColor White
Write-Host "   💾 Commits: $commitCount" -ForegroundColor White
Write-Host "   🌿 Branch: master" -ForegroundColor White

Write-Host ""
Write-Host "🔗 Tu repositorio estará disponible en:" -ForegroundColor Cyan
Write-Host "   $repoUrl" -ForegroundColor White
Write-Host ""

Write-Host "📋 Próximos pasos recomendados:" -ForegroundColor Yellow
Write-Host "   • Configurar GitHub Pages (si quieres documentación web)" -ForegroundColor White
Write-Host "   • Configurar GitHub Actions para CI/CD" -ForegroundColor White  
Write-Host "   • Agregar colaboradores si es necesario" -ForegroundColor White
Write-Host "   • Configurar issues y labels" -ForegroundColor White
Write-Host ""

Write-Host "🎊 ¡KarenVision está ahora en GitHub!" -ForegroundColor Green