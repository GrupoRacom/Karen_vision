# 🏪 KarenVision - Sistema de Pedidos con Publicidad

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-orange.svg)](https://www.mysql.com/)
[![WPF](https://img.shields.io/badge/WPF-Windows-lightblue.svg)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

## 📋 Descripción

**KarenVision** es una innovadora aplicación de escritorio desarrollada en C# WPF que implementa un sistema de pedidos con publicidad interactiva obligatoria. Los usuarios deben ver publicidad antes de acceder al sistema de pedidos, creando una experiencia única que combina marketing efectivo con funcionalidad comercial.

## ✨ Características Principales

### 🎯 Funcionalidades Core
- **🔒 Publicidad Obligatoria**: Sistema de bloqueo que requiere visualización completa de anuncios
- **🛒 Sistema de Pedidos Completo**: Selección de productos, gestión de carrito y procesamiento
- **👥 Gestión de Clientes**: Registro, búsqueda y validación de datos de clientes
- **💾 Base de Datos MySQL**: Soporte completo con Entity Framework Core
- **🎨 Interfaz Moderna**: Diseño WPF atractivo con estilos personalizados

### 🛡️ Seguridad y Robustez
- **⚡ Manejo de Errores**: Logging detallado y manejo robusto de excepciones
- **✅ Validación de Datos**: Verificación de stock, clientes y productos
- **🔄 Transacciones**: Consistencia de datos garantizada
- **🔐 Configuración Segura**: Cadenas de conexión parametrizadas

## 🏗️ Arquitectura del Proyecto

```
Karen_Vision/
├── 📁 Models/                 # Entidades de datos
│   ├── Producto.cs           # Modelo de productos
│   ├── Cliente.cs            # Modelo de clientes  
│   ├── Pedido.cs             # Modelo de pedidos
│   └── DetallePedido.cs      # Detalles de pedido
├── 📁 Data/                  # Contexto de base de datos
│   └── KarenVisionContext.cs # Context de Entity Framework
├── 📁 Services/              # Lógica de negocio
│   ├── Interfaces/           # Contratos de servicios
│   ├── ProductoService.cs    # Servicio de productos
│   ├── ClienteService.cs     # Servicio de clientes
│   └── PedidoService.cs      # Servicio de pedidos
├── 📁 Views/                 # Interfaces WPF
│   ├── MainWindow.xaml       # Ventana principal
│   ├── PublicidadWindow.xaml # Pantalla de publicidad
│   └── PedidosWindow.xaml    # Sistema de pedidos
├── 📁 Scripts/               # Scripts de base de datos
│   ├── configurar_usuario.sql    # Configuración MySQL
│   ├── crear_base_datos.sql      # Estructura de BD
│   ├── datos_ejemplo.sql         # Datos de prueba
│   └── setup_mysql.ps1           # Script automático
└── 📁 Documentation/         # Documentación
    ├── CONFIGURACION_MYSQL.md
    └── ESTADO_FINAL.md
```

## 🚀 Inicio Rápido

### 📋 Requisitos Previos

- **.NET 8.0 SDK** o superior
- **MySQL 8.0+** instalado y ejecutándose
- **Windows 10/11** (para WPF)
- **Visual Studio Code** o **Visual Studio** (recomendado)

### ⚡ Instalación Rápida

1. **Clonar el repositorio**
```bash
git clone https://github.com/TU_USUARIO/Karen_Vision.git
cd Karen_Vision
```

2. **Configurar MySQL (Automático)**
```powershell
.\Scripts\setup_mysql.ps1
```

3. **Ejecutar la aplicación**
```bash
dotnet run
```

### 🔧 Configuración Manual de MySQL

Si prefieres configurar manualmente:

```bash
# 1. Crear usuario y base de datos
mysql -u root -p < Scripts/configurar_usuario.sql

# 2. Crear estructura de tablas
mysql -u root -p < Scripts/crear_base_datos.sql  

# 3. Insertar datos de ejemplo
mysql -u pvtouch -pfamilia karen_local < Scripts/datos_ejemplo.sql
```

## 📊 Base de Datos

### Configuración por Defecto
- **Base de datos**: `karen_local`
- **Usuario**: `pvtouch`
- **Password**: `familia`
- **Servidor**: `localhost:3306`

### Estructura de Tablas
- **`clientes`** - Información de clientes con ID único
- **`productos`** - Catálogo con precios y stock
- **`pedidos`** - Órdenes con estado y totales  
- **`detalles_pedido`** - Items individuales por pedido

## 🎮 Uso de la Aplicación

### Flujo Principal
1. **🏠 Inicio**: Ventana principal con opciones de navegación
2. **📺 Publicidad**: Visualización obligatoria de anuncios (configurable)
3. **🔓 Acceso**: Una vez completada la publicidad, acceso al sistema
4. **🛍️ Pedidos**: Selección de productos, datos del cliente y confirmación

### Capturas de Pantalla

| Ventana Principal | Publicidad | Sistema de Pedidos |
|:---:|:---:|:---:|
| ![Main](docs/images/main-window.png) | ![Ad](docs/images/ad-window.png) | ![Orders](docs/images/orders-window.png) |

## 🛠️ Desarrollo

### Compilar el Proyecto
```bash
dotnet build
```

### Ejecutar Tests (cuando estén disponibles)
```bash
dotnet test
```

### Publicar para Producción
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

## 📖 Documentación Adicional

- [📋 Configuración de MySQL](CONFIGURACION_MYSQL.md)
- [🎯 Estado del Proyecto](ESTADO_FINAL.md)  
- [🔧 Guía de Desarrollo](docs/DESARROLLO.md)
- [📝 Notas de Versión](CHANGELOG.md)

## 🤝 Contribuir

¡Las contribuciones son bienvenidas! Por favor:

1. Haz fork del proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📋 Roadmap

- [ ] Tests unitarios
- [ ] Reportes de ventas
- [ ] Múltiples tipos de publicidad
- [ ] Soporte para PostgreSQL
- [ ] API REST
- [ ] Aplicación móvil

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para detalles.

## 👨‍💻 Autor

**Desarrollado con ❤️ por el equipo de KarenVision**

- 📧 Email: contacto@karenvision.com
- 🐦 Twitter: [@KarenVisionApp](https://twitter.com/KarenVisionApp)
- 💼 LinkedIn: [KarenVision](https://linkedin.com/company/karenvision)

## 🙏 Reconocimientos

- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [MySQL](https://www.mysql.com/)
- [.NET Community](https://dotnet.microsoft.com/platform/community)
- [WPF Community](https://github.com/dotnet/wpf)

---

⭐ **¡Si te gusta este proyecto, dale una estrella!** ⭐

![KarenVision Logo](docs/images/karen-vision-banner.png)