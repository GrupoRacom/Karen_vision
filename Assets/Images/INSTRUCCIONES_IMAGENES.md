# 📸 GUÍA DE IMÁGENES PARA PUBLICIDAD - KarenVision

## 📁 Ubicación de las Imágenes

**Carpeta**: `Assets/Images/Publicidad/`

Coloca tus 5 imágenes de publicidad en esta carpeta con los siguientes nombres:

```
Assets/Images/Publicidad/
├── publicidad_1.jpg
├── publicidad_2.jpg  
├── publicidad_3.jpg
├── publicidad_4.jpg
└── publicidad_5.jpg
```

## 📏 Especificaciones Recomendadas

### Formato de Imágenes
- **Formato**: JPG, PNG, o BMP
- **Resolución**: 800x600 píxeles (o proporción 4:3)
- **Tamaño máximo**: 2 MB por imagen
- **Orientación**: Horizontal (landscape)

### Nombres de Archivo
Los nombres deben ser exactamente:
- `publicidad_1.jpg`
- `publicidad_2.jpg`
- `publicidad_3.jpg`
- `publicidad_4.jpg`
- `publicidad_5.jpg`

## ⚙️ Configuración de Tiempo

En `appsettings.json` puedes configurar:

```json
{
  "AppSettings": {
    "AdvertisementDuration": 10,
    "ImageChangeInterval": 3,
    "RequireAdvertisementInteraction": true
  }
}
```

- **AdvertisementDuration**: Tiempo total de publicidad (segundos)
- **ImageChangeInterval**: Tiempo entre cambio de imágenes (segundos)

## 🎨 Consejos para las Imágenes

### Contenido Sugerido
1. **publicidad_1.jpg**: Logo de la empresa o bienvenida
2. **publicidad_2.jpg**: Productos destacados
3. **publicidad_3.jpg**: Ofertas especiales
4. **publicidad_4.jpg**: Servicios adicionales
5. **publicidad_5.jpg**: Información de contacto

### Diseño
- Usa colores atractivos y contrastantes
- Incluye texto legible (mínimo 24px)
- Evita saturar con mucha información
- Mantén consistencia visual entre imágenes

## 🔧 Formatos Soportados

- **JPG/JPEG** ✅ (Recomendado para fotos)
- **PNG** ✅ (Recomendado para gráficos con transparencia)
- **BMP** ✅ (Para imágenes simples)
- **GIF** ✅ (Animaciones simples)

## 📦 Ejemplo de Estructura Final

```
Karen_Vision/
├── Assets/
│   └── Images/
│       └── Publicidad/
│           ├── publicidad_1.jpg (Tu imagen 1)
│           ├── publicidad_2.jpg (Tu imagen 2)
│           ├── publicidad_3.jpg (Tu imagen 3)
│           ├── publicidad_4.jpg (Tu imagen 4)
│           └── publicidad_5.jpg (Tu imagen 5)
├── Views/
│   └── PublicidadWindow.xaml (Actualizado para mostrar imágenes)
└── appsettings.json (Configuración de tiempos)
```

## 🚀 Siguiente Paso

1. **Agrega tus 5 imágenes** en la carpeta `Assets/Images/Publicidad/`
2. **Nombra los archivos** exactamente como se indica
3. **La aplicación automáticamente** las detectará y mostrará

¡El sistema está configurado para cambiar automáticamente entre las 5 imágenes durante el período de publicidad!