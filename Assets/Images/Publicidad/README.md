# Imágenes de Publicidad - KarenVision

## 📁 Ubicación de las Imágenes

Coloca tus imágenes publicitarias en esta carpeta:
`Assets/Images/Publicidad/`

## 🖼️ Formatos Soportados

- `.jpg` / `.jpeg`
- `.png`
- `.bmp`
- `.gif`

## 📝 Nombres de Archivo Recomendados

Para mejor organización, usa nombres descriptivos:
- `publicidad1.jpg`
- `oferta-especial.png`
- `producto-destacado.jpg`
- `promocion-mes.png`
- `banner-principal.jpg`

## ⚙️ Configuración

El cambio de imágenes está controlado por la configuración en `appsettings.json`:

```json
{
  "AppSettings": {
    "ImageChangeInterval": 3,  // Segundos entre cambios
    "AdvertisementDuration": 15  // Duración total de la publicidad
  }
}
```

## 📏 Recomendaciones de Tamaño

- **Resolución recomendada**: 1920x1080 (Full HD)
- **Formato**: 16:9 (widescreen)
- **Tamaño de archivo**: Máximo 2MB por imagen para mejor rendimiento

## 🎨 Consejos de Diseño

1. **Texto legible**: Usa fuentes grandes y contrastantes
2. **Colores llamativos**: Para captar la atención
3. **Call-to-action claro**: "¡Compra ahora!", "Oferta limitada", etc.
4. **Branding consistente**: Mantén la identidad visual
5. **Mensaje simple**: Fácil de leer en pocos segundos

## 🔄 Rotación Automática

Las imágenes cambiarán automáticamente cada 3 segundos (configurable).
- Si solo hay 1 imagen, se mostrará durante toda la publicidad
- Con múltiples imágenes, rotarán en orden
- El sistema busca automáticamente todas las imágenes en esta carpeta

## 📱 Ejemplo de Uso

1. Guarda tus 5 imágenes en `Assets/Images/Publicidad/`
2. La aplicación las detectará automáticamente
3. Se mostrarán rotando cada 3 segundos
4. El usuario verá toda la secuencia durante la duración configurada