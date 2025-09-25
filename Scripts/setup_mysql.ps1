# Script de PowerShell para configurar MySQL para KarenVision
# Configuración de base de datos: karen_local
# Usuario: pvtouch
# Password: familia

Write-Host "=== Configuración de MySQL para KarenVision ===" -ForegroundColor Green
Write-Host ""

# Verificar si MySQL está instalado
try {
    $mysqlVersion = mysql --version 2>$null
    if ($mysqlVersion) {
        Write-Host "✅ MySQL encontrado: $mysqlVersion" -ForegroundColor Green
    }
} catch {
    Write-Host "❌ MySQL no encontrado. Por favor instala MySQL 8.0+ primero." -ForegroundColor Red
    Write-Host "Descarga desde: https://dev.mysql.com/downloads/mysql/" -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "📋 Configuración a aplicar:" -ForegroundColor Cyan
Write-Host "  - Base de datos: karen_local" -ForegroundColor White
Write-Host "  - Usuario: pvtouch" -ForegroundColor White
Write-Host "  - Password: familia" -ForegroundColor White
Write-Host ""

# Solicitar credenciales de root
$rootPassword = Read-Host "Ingresa la contraseña de root de MySQL" -AsSecureString
$rootPasswordPlain = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($rootPassword))

Write-Host ""
Write-Host "🔧 Ejecutando configuración..." -ForegroundColor Yellow

# Ejecutar script de usuario
Write-Host "1. Configurando usuario de base de datos..." -ForegroundColor Cyan
$configUserScript = Get-Content -Path "Scripts\configurar_usuario.sql" -Raw
$configUserScript | mysql -u root -p$rootPasswordPlain

if ($LASTEXITCODE -eq 0) {
    Write-Host "   ✅ Usuario configurado correctamente" -ForegroundColor Green
} else {
    Write-Host "   ❌ Error configurando usuario" -ForegroundColor Red
}

# Ejecutar script de creación de base de datos
Write-Host "2. Creando base de datos y tablas..." -ForegroundColor Cyan
mysql -u root -p$rootPasswordPlain < "Scripts\crear_base_datos.sql"

if ($LASTEXITCODE -eq 0) {
    Write-Host "   ✅ Base de datos creada correctamente" -ForegroundColor Green
} else {
    Write-Host "   ❌ Error creando base de datos" -ForegroundColor Red
}

# Ejecutar datos de ejemplo
Write-Host "3. Insertando datos de ejemplo..." -ForegroundColor Cyan
mysql -u pvtouch -pfamilia karen_local < "Scripts\datos_ejemplo.sql" 2>$null

if ($LASTEXITCODE -eq 0) {
    Write-Host "   ✅ Datos de ejemplo insertados" -ForegroundColor Green
} else {
    Write-Host "   ⚠️  Advertencia: No se pudieron insertar datos de ejemplo (puede ser normal)" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "🧪 Probando conexión..." -ForegroundColor Cyan
$testQuery = "SELECT COUNT(*) as clientes FROM clientes; SELECT COUNT(*) as productos FROM productos;"
echo $testQuery | mysql -u pvtouch -pfamilia karen_local

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Conexión de prueba exitosa" -ForegroundColor Green
} else {
    Write-Host "❌ Error en conexión de prueba" -ForegroundColor Red
}

Write-Host ""
Write-Host "🎉 Configuración de MySQL completada!" -ForegroundColor Green
Write-Host ""
Write-Host "📝 Resumen de configuración:" -ForegroundColor Cyan
Write-Host "  - Servidor: localhost" -ForegroundColor White
Write-Host "  - Base de datos: karen_local" -ForegroundColor White
Write-Host "  - Usuario: pvtouch" -ForegroundColor White
Write-Host "  - Password: familia" -ForegroundColor White
Write-Host ""
Write-Host "🚀 Ahora puedes ejecutar la aplicación con:" -ForegroundColor Yellow
Write-Host "   dotnet run" -ForegroundColor White
Write-Host ""