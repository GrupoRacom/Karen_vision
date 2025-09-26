# Script de PowerShell para subir KarenVision a GitHub
# Automatiza la creacion del repositorio y la subida del codigo

Write-Host "Script de GitHub para KarenVision" -ForegroundColor Green
Write-Host "======================================" -ForegroundColor Green
Write-Host ""

# Verificar que estamos en el directorio correcto
if (-not (Test-Path "KarenVision.csproj")) {
    Write-Host "Error: No se encuentra KarenVision.csproj en el directorio actual" -ForegroundColor Red
    Write-Host "Por favor ejecuta este script desde el directorio del proyecto" -ForegroundColor Yellow
    exit 1
}

Write-Host "Directorio del proyecto verificado" -ForegroundColor Green

# Verificar que Git este inicializado
try {
    $gitStatus = git status 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Error: Git no esta inicializado" -ForegroundColor Red
        exit 1
    }
    Write-Host "Repositorio Git inicializado" -ForegroundColor Green
} catch {
    Write-Host "Error: Git no esta disponible" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Pasos para subir a GitHub:" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Ir a https://github.com/new" -ForegroundColor White
Write-Host "2. Crear repositorio con nombre: Karen_Vision" -ForegroundColor White  
Write-Host "3. NO inicializar con README (ya lo tenemos)" -ForegroundColor White
Write-Host "4. Copiar la URL del repositorio" -ForegroundColor White
Write-Host ""

# Solicitar URL del repositorio
do {
    $repoUrl = Read-Host "Pega la URL de tu repositorio GitHub (ej: https://github.com/usuario/Karen_Vision.git)"
    if ([string]::IsNullOrWhiteSpace($repoUrl)) {
        Write-Host "La URL no puede estar vacia" -ForegroundColor Yellow
    }
} while ([string]::IsNullOrWhiteSpace($repoUrl))

Write-Host ""
Write-Host "Configurando remote origin..." -ForegroundColor Cyan

try {
    # Agregar el remote origin
    git remote add origin $repoUrl
    Write-Host "Remote origin configurado" -ForegroundColor Green
} catch {
    # Si ya existe, actualizarlo
    git remote set-url origin $repoUrl
    Write-Host "Remote origin actualizado" -ForegroundColor Green
}

Write-Host ""
Write-Host "Subiendo codigo a GitHub..." -ForegroundColor Cyan

try {
    # Hacer push al repositorio
    git push -u origin master
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Codigo subido exitosamente!" -ForegroundColor Green
    } else {
        Write-Host "Error al subir el codigo" -ForegroundColor Red
        Write-Host "Intenta manualmente: git push -u origin master" -ForegroundColor Yellow
        exit 1
    }
} catch {
    Write-Host "Error al subir el codigo" -ForegroundColor Red
    Write-Host "Intenta manualmente: git push -u origin master" -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "EXITO! Tu proyecto KarenVision esta en GitHub" -ForegroundColor Green
Write-Host ""
Write-Host "Estadisticas del repositorio:" -ForegroundColor Cyan

# Mostrar estadisticas
$commitCount = git rev-list --count HEAD
$fileCount = (git ls-files | Measure-Object).Count

Write-Host "   Archivos: $fileCount" -ForegroundColor White
Write-Host "   Commits: $commitCount" -ForegroundColor White
Write-Host "   Branch: master" -ForegroundColor White

Write-Host ""
Write-Host "Tu repositorio estara disponible en:" -ForegroundColor Cyan
Write-Host "   $repoUrl" -ForegroundColor White
Write-Host ""

Write-Host "Proximos pasos recomendados:" -ForegroundColor Yellow
Write-Host "   • Configurar GitHub Pages (si quieres documentacion web)" -ForegroundColor White
Write-Host "   • Configurar GitHub Actions para CI/CD" -ForegroundColor White  
Write-Host "   • Agregar colaboradores si es necesario" -ForegroundColor White
Write-Host "   • Configurar issues y labels" -ForegroundColor White
Write-Host ""

Write-Host "KarenVision esta ahora en GitHub!" -ForegroundColor Green