# Changelog

Todas las modificaciones notables a este proyecto serán documentadas en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-es/1.0.0/),
y este proyecto adhiere al [Versionado Semántico](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2025-09-25

### ✨ Añadido
- Sistema de publicidad obligatoria con timer configurable
- Sistema completo de gestión de pedidos
- CRUD completo de clientes con validación de datos únicos
- Catálogo de productos con control de inventario
- Carrito de compras con cálculo automático de totales
- Integración completa con MySQL usando Entity Framework Core
- Interfaz WPF moderna con estilos personalizados
- Inyección de dependencias con Microsoft.Extensions
- Manejo robusto de errores y logging
- Scripts automáticos de configuración de base de datos
- Documentación completa del proyecto

### 🔧 Técnico
- Framework .NET 8.0 con WPF
- Entity Framework Core 8.0.8
- Pomelo MySQL Provider
- Patrón de arquitectura en capas (Models, Data, Services, Views)
- Separación de responsabilidades con interfaces
- Configuración mediante appsettings.json
- Scripts PowerShell para automatización

### 📊 Base de Datos
- Tablas: `clientes`, `productos`, `pedidos`, `detalles_pedido`
- Relaciones con integridad referencial
- Índices optimizados para consultas
- Datos de ejemplo incluidos
- Usuario específico: `pvtouch` en base de datos `karen_local`

### 📋 Documentación
- README completo con instrucciones de instalación
- Guía de configuración de MySQL
- Scripts de automatización
- Documentación de arquitectura
- Archivo de licencia MIT

### 🎯 Funcionalidades
- Publicidad interactiva con contador visual
- Búsqueda y filtrado de productos
- Gestión completa del carrito de compras
- Validación de stock en tiempo real
- Registro de pedidos con detalles
- Interfaz intuitiva y moderna

## [Unreleased]

### 🚀 Próximas Funcionalidades
- Tests unitarios completos
- Reportes de ventas y analytics
- Múltiples tipos de publicidad
- Soporte para PostgreSQL
- API REST para integración
- Aplicación móvil complementaria
- Sistema de notificaciones
- Backup automático de datos

### 🔧 Mejoras Planificadas
- Optimización de rendimiento
- Internacionalización (i18n)
- Tema oscuro/claro
- Configuración de UI personalizable
- Integración con servicios de pago
- Sistema de usuarios y permisos

---

### Formato de Versiones
- **[Major.Minor.Patch]** - Fecha de lanzamiento
- **Major**: Cambios incompatibles en API
- **Minor**: Funcionalidades nuevas compatibles
- **Patch**: Corrección de errores compatibles

### Tipos de Cambios
- **✨ Añadido**: Nuevas funcionalidades
- **🔧 Cambiado**: Cambios en funcionalidades existentes  
- **❌ Obsoleto**: Funcionalidades que serán removidas
- **🗑️ Removido**: Funcionalidades removidas
- **🐛 Corregido**: Corrección de errores
- **🔒 Seguridad**: Mejoras de seguridad