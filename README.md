# Karen Vision - Sistema de Pedidos con Publicidad

## Descripción

Karen Vision es una aplicación de escritorio desarrollada en C# WPF que implementa un sistema de pedidos con publicidad interactiva. La aplicación requiere que los usuarios vean publicidad antes de poder acceder al sistema de pedidos, creando una experiencia única que combina marketing y funcionalidad comercial.

## Características Principales

### 🎯 Funcionalidades Core
- **Pantalla de publicidad obligatoria**: Los usuarios deben interactuar con la publicidad antes de acceder al sistema
- **Sistema de pedidos completo**: Selección de productos, gestión de carrito y procesamiento de pedidos
- **Gestión de clientes**: Registro y búsqueda de clientes con validación de datos
- **Conexión a base de datos MySQL**: Soporte para modo local y remoto con configuración flexible
- **Interfaz moderna WPF**: Diseño atractivo y responsivo con animaciones

### 🛡️ Seguridad y Robustez
- **Manejo de errores robusto**: Logging detallado y manejo de excepciones
- **Validación de datos**: Verificación de stock, validación de clientes y productos
- **Transacciones de base de datos**: Consistencia de datos garantizada
- **Configuración segura**: Cadenas de conexión configurables y parametrizadas

## Estructura del Proyecto

```
Karen_Vision/
├── Models/                 # Entidades de datos
│   ├── Producto.cs        # Modelo de productos
│   ├── Cliente.cs         # Modelo de clientes
│   ├── Pedido.cs          # Modelo de pedidos
│   └── DetallePedido.cs   # Modelo de detalles de pedido
├── Data/                   # Contexto de base de datos
│   └── KarenVisionContext.cs
├── Services/               # Lógica de negocio
│   ├── IProductoService.cs & ProductoService.cs
│   ├── IClienteService.cs & ClienteService.cs
│   └── IPedidoService.cs & PedidoService.cs
├── Views/                  # Interfaces de usuario
│   ├── MainWindow.xaml    # Ventana principal
│   ├── PublicidadWindow.xaml # Ventana de publicidad
│   └── PedidosWindow.xaml # Sistema de pedidos
├── Scripts/                # Scripts de base de datos
│   └── crear_base_datos.sql
├── appsettings.json       # Configuración de la aplicación
└── KarenVision.csproj     # Archivo de proyecto
```

## Requisitos del Sistema

### Software Requerido
- **.NET 8.0** o superior
- **MySQL Server 8.0** o superior
- **Visual Studio Code** con extensiones de C#
- **Windows 10** o superior

### Dependencias NuGet
- `Microsoft.EntityFrameworkCore` (8.0.8)
- `Pomelo.EntityFrameworkCore.MySql` (8.0.2)
- `Microsoft.Extensions.Configuration` (8.0.0)
- `Microsoft.Extensions.DependencyInjection` (8.0.0)
- `Microsoft.Extensions.Hosting` (8.0.0)
- `Microsoft.Extensions.Logging` (8.0.0)

## Instalación y Configuración

### 1. Configuración de la Base de Datos

```bash
# Conectarse a MySQL como administrador
mysql -u root -p

# Ejecutar el script de configuración automática (RECOMENDADO)
.\Scripts\setup_mysql.ps1

# O configuración manual:
# 1. Crear usuario y base de datos
mysql -u root -p < Scripts/configurar_usuario.sql

# 2. Crear tablas y estructura
mysql -u root -p < Scripts/crear_base_datos.sql

# 3. Insertar datos de ejemplo
mysql -u pvtouch -pfamilia karen_local < Scripts/datos_ejemplo.sql
```

### 2. Configuración de la Aplicación

La configuración ya está establecida en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=karen_local;Uid=pvtouch;Pwd=familia;SslMode=none;"
  },
  "AppSettings": {
    "AdvertisementDuration": 10,
    "RequireAdvertisementInteraction": true
  }
}
```

**Configuración Actual:**
- **Base de datos**: `karen_local`
- **Usuario**: `pvtouch`  
- **Password**: `familia`
- **Servidor**: `localhost`

### 3. Compilación y Ejecución

```bash
# Restaurar dependencias
dotnet restore

# Compilar el proyecto
dotnet build

# Ejecutar la aplicación
dotnet run
```

## Uso de la Aplicación

### Flujo Principal

1. **Inicio**: La aplicación muestra la ventana principal con opciones de navegación
2. **Publicidad**: El usuario debe ver la publicidad completa o interactuar con ella
3. **Acceso Desbloqueado**: Una vez vista la publicidad, se habilita el acceso al sistema de pedidos
4. **Gestión de Pedidos**: Selección de productos, datos del cliente y confirmación de pedidos

### Funcionalidades Detalladas

#### Sistema de Publicidad
- **Duración configurable**: Tiempo de visualización personalizable
- **Interacción opcional**: Los usuarios pueden interactuar para obtener información adicional
- **Contador visual**: Progreso y tiempo restante claramente visible
- **Prevención de omisión**: No se puede cerrar hasta completar el tiempo mínimo

#### Sistema de Pedidos
- **Búsqueda de productos**: Filtrado por nombre con resultados en tiempo real
- **Gestión de carrito**: Agregar, modificar cantidades y eliminar productos
- **Validación de stock**: Verificación automática de disponibilidad
- **Datos del cliente**: Registro de nuevos clientes o búsqueda de existentes
- **Confirmación de pedidos**: Proceso guiado con resumen y confirmación final

## Configuración de Base de Datos

### Estructura de Tablas

La base de datos incluye las siguientes tablas principales:

- **clientes**: Información de clientes con ID de identificación único
- **productos**: Catálogo de productos con precios y stock
- **pedidos**: Registros de pedidos con estados y totales
- **detalles_pedido**: Líneas de pedido con productos y cantidades

### Datos de Ejemplo

El script incluye datos de prueba:
- 15 productos tecnológicos con precios y stock
- 5 clientes de ejemplo
- 5 pedidos con diferentes estados
- Detalles completos de pedidos

## Arquitectura Técnica

### Patrones Implementados

- **Repository Pattern**: A través de servicios para acceso a datos
- **Dependency Injection**: Configuración completa con Microsoft.Extensions
- **MVVM Pattern**: Separación de lógica y presentación en WPF
- **Unit of Work**: Transacciones coordinadas en operaciones complejas

### Logging y Monitoreo

- **Structured Logging**: Logs detallados con diferentes niveles
- **Error Handling**: Manejo robusto de excepciones
- **Performance Tracking**: Monitoreo de operaciones de base de datos

## Personalización

### Configuración de Publicidad

```json
"AppSettings": {
    "AdvertisementDuration": 15,        // Duración en segundos
    "RequireAdvertisementInteraction": false  // Si requiere interacción
}
```

### Configuración de Base de Datos

#### Modo Local
```json
"DefaultConnection": "Server=localhost;Database=KarenVisionDB;Uid=root;Pwd=password;SslMode=none;"
```

#### Modo Remoto
```json
"DefaultConnection": "Server=mi-servidor.com;Port=3306;Database=KarenVisionDB;Uid=usuario;Pwd=password;SslMode=Required;"
```

## Mantenimiento

### Backup de Base de Datos

```bash
mysqldump -u root -p KarenVisionDB > backup_karenvision.sql
```

### Actualización de Stock

```sql
UPDATE productos SET stock = stock + 10 WHERE nombre = 'Laptop Dell Inspiron 15';
```

### Consultas Útiles

```sql
-- Pedidos por estado
SELECT estado, COUNT(*) as total FROM pedidos GROUP BY estado;

-- Productos más vendidos
SELECT p.nombre, SUM(dp.cantidad) as total_vendido
FROM productos p
JOIN detalles_pedido dp ON p.id = dp.producto_id
GROUP BY p.id, p.nombre
ORDER BY total_vendido DESC;

-- Clientes con más pedidos
SELECT c.nombre_completo, COUNT(pe.id) as total_pedidos
FROM clientes c
LEFT JOIN pedidos pe ON c.id = pe.cliente_id
GROUP BY c.id, c.nombre_completo
ORDER BY total_pedidos DESC;
```

## Desarrollo y Contribución

### Extensiones Recomendadas para VS Code

- C# for Visual Studio Code
- .NET Extension Pack
- MySQL (para gestión de base de datos)
- GitLens (para control de versiones)

### Estructura de Desarrollo

1. **Models**: Agregar nuevas entidades siguiendo el patrón existente
2. **Services**: Implementar lógica de negocio con interfaces
3. **Views**: Crear nuevas ventanas siguiendo los estilos establecidos
4. **Data**: Configurar relaciones en el contexto de Entity Framework

## Solución de Problemas Comunes

### Error de Conexión a Base de Datos
- Verificar que MySQL esté ejecutándose
- Confirmar credenciales en `appsettings.json`
- Verificar que la base de datos exista

### Problemas de Stock
- Los productos con stock 0 no pueden agregarse al carrito
- Verificar datos en la tabla `productos`

### Errores de Validación
- Los campos obligatorios deben completarse
- El ID de identificación debe ser único por cliente

## Información de Contacto y Soporte

Para soporte técnico o consultas sobre el desarrollo:

- **Documentación**: Consultar comentarios en el código fuente
- **Logs**: Revisar archivos de log para diagnóstico de errores
- **Base de datos**: Verificar integridad de datos regularmente

## Licencia

Este proyecto es desarrollado como ejemplo educativo y comercial. Todos los derechos reservados.

---

**Karen Vision v1.0** - Sistema de Pedidos con Publicidad Interactiva
Desarrollado con ❤️ usando C# WPF y MySQL