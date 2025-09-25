# Guía de Contribución - KarenVision

¡Gracias por tu interés en contribuir a KarenVision! 🎉

## 📋 Cómo Contribuir

### 🐛 Reportar Bugs

1. Verifica que el bug no haya sido reportado antes en [Issues](https://github.com/TU_USUARIO/Karen_Vision/issues)
2. Abre un nuevo issue con la etiqueta "bug"
3. Incluye información detallada:
   - Pasos para reproducir el error
   - Comportamiento esperado vs actual
   - Screenshots si aplica
   - Información del sistema (Windows version, .NET version)

### ✨ Sugerir Nuevas Funcionalidades

1. Abre un issue con la etiqueta "enhancement"
2. Describe claramente la funcionalidad propuesta
3. Explica por qué sería útil para los usuarios
4. Proporciona ejemplos de uso si es posible

### 🔧 Pull Requests

1. **Fork el repositorio**
2. **Crea una rama** para tu feature:
   ```bash
   git checkout -b feature/nueva-funcionalidad
   ```
3. **Realiza tus cambios** siguiendo las guías de estilo
4. **Ejecuta los tests** (cuando estén disponibles)
5. **Commit tus cambios** con mensajes descriptivos
6. **Push a tu fork** y abre un Pull Request

### 📝 Estándares de Código

#### C# / .NET
- Usa PascalCase para clases y métodos públicos
- Usa camelCase para variables locales y parámetros
- Usa async/await para operaciones asíncronas
- Incluye comentarios XML para métodos públicos
- Sigue las convenciones de .NET

#### XAML
- Usa PascalCase para nombres de controles
- Mantén la consistencia en el espaciado
- Usa estilos definidos en App.xaml

#### Base de Datos
- Nombres de tablas en minúsculas con guiones bajos
- Usa claves primarias auto-incrementales
- Mantén la integridad referencial

### 🧪 Tests

Aunque los tests unitarios están en el roadmap, por ahora asegúrate de:
- Compilar sin errores ni warnings
- Probar manualmente todas las funcionalidades afectadas
- Verificar que la conexión a la base de datos funciona

### 📖 Documentación

- Actualiza la documentación relevante
- Incluye comentarios en el código para funcionalidades complejas
- Actualiza el CHANGELOG.md para cambios notables

## 🏗️ Configuración del Entorno de Desarrollo

### Requisitos
- .NET 8.0 SDK
- MySQL 8.0+
- Visual Studio Code o Visual Studio
- Git

### Setup
```bash
git clone https://github.com/TU_USUARIO/Karen_Vision.git
cd Karen_Vision
.\Scripts\setup_mysql.ps1
dotnet build
dotnet run
```

## 🏷️ Etiquetas de Issues

- `bug` - Error en el código
- `enhancement` - Nueva funcionalidad
- `documentation` - Mejoras en documentación
- `good first issue` - Perfecto para nuevos contribuidores
- `help wanted` - Necesitamos ayuda con esto
- `question` - Pregunta sobre el proyecto

## 📞 Contacto

Si tienes preguntas, no dudes en:
- Abrir un issue
- Contactarnos en developer@karenvision.com

¡Esperamos tus contribuciones! 🚀