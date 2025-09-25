# Configuración de MySQL para KarenVision

## Configuración Requerida
- **Base de datos**: `karen_local`
- **Usuario**: `pvtouch`
- **Password**: `familia`
- **Servidor**: `localhost` (por defecto)

## Opción 1: Configuración Automática (Recomendada)

### Usando PowerShell (Windows)
```powershell
# Navegar al directorio del proyecto
cd "d:\1 EstudiaProgramando\Karen_Vision"

# Ejecutar script de configuración automática
.\Scripts\setup_mysql.ps1
```

El script automáticamente:
1. Verifica que MySQL esté instalado
2. Crea el usuario `pvtouch` con password `familia`
3. Crea la base de datos `karen_local`
4. Crea todas las tablas necesarias (clientes, productos, pedidos, detalles_pedido)
5. Inserta datos de ejemplo
6. Verifica la conexión

## Opción 2: Configuración Manual

### Paso 1: Conectar a MySQL como root
```bash
mysql -u root -p
```

### Paso 2: Configurar usuario (ejecutar como root)
```sql
-- Ejecutar el contenido de Scripts/configurar_usuario.sql
source Scripts/configurar_usuario.sql;
```

### Paso 3: Crear base de datos y tablas
```sql
-- Ejecutar el contenido de Scripts/crear_base_datos.sql
source Scripts/crear_base_datos.sql;
```

### Paso 4: Insertar datos de ejemplo
```sql
-- Ejecutar el contenido de Scripts/datos_ejemplo.sql
source Scripts/datos_ejemplo.sql;
```

## Verificación de la Configuración

### Probar conexión con el usuario de la aplicación
```bash
mysql -u pvtouch -pfamilia karen_local
```

### Verificar tablas creadas
```sql
USE karen_local;
SHOW TABLES;

-- Verificar datos
SELECT COUNT(*) as total_clientes FROM clientes;
SELECT COUNT(*) as total_productos FROM productos;
```

## Estructura de Tablas Creadas

### Tabla `clientes`
- `id` (PK, AUTO_INCREMENT)
- `id_identificacion` (VARCHAR(50), UNIQUE)
- `nombre_completo` (VARCHAR(255))
- `email` (VARCHAR(255))
- `telefono` (VARCHAR(20))
- `direccion` (TEXT)
- `fecha_creacion` (DATETIME)
- `ultimo_pedido` (DATETIME)

### Tabla `productos`
- `id` (PK, AUTO_INCREMENT)
- `nombre` (VARCHAR(255))
- `descripcion` (TEXT)
- `precio` (DECIMAL(18,2))
- `stock` (INT)
- `imagen_url` (VARCHAR(500))
- `activo` (BOOLEAN)
- `fecha_creacion` (DATETIME)

### Tabla `pedidos`
- `id` (PK, AUTO_INCREMENT)
- `cliente_id` (INT, FK)
- `fecha_pedido` (DATETIME)
- `total` (DECIMAL(18,2))
- `estado` (VARCHAR(50))
- `observaciones` (TEXT)

### Tabla `detalles_pedido`
- `id` (PK, AUTO_INCREMENT)
- `pedido_id` (INT, FK)
- `producto_id` (INT, FK)
- `cantidad` (INT)
- `precio_unitario` (DECIMAL(18,2))
- `subtotal` (DECIMAL(18,2))

## Configuración de la Aplicación

La configuración ya está actualizada en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=karen_local;Uid=pvtouch;Pwd=familia;SslMode=none;"
  }
}
```

## Datos de Ejemplo Incluidos

- **5 clientes** de prueba con información completa
- **10 productos** variados con precios y stock
- **5 pedidos** de ejemplo con sus detalles

## Ejecutar la Aplicación

Una vez configurada la base de datos:
```bash
cd "d:\1 EstudiaProgramando\Karen_Vision"
dotnet run
```

## Solución de Problemas

### Error de conexión
1. Verificar que MySQL esté ejecutándose
2. Verificar credenciales en `appsettings.json`
3. Verificar que el usuario `pvtouch` tenga permisos

### El usuario no existe
```sql
-- Crear usuario manualmente
CREATE USER 'pvtouch'@'localhost' IDENTIFIED BY 'familia';
GRANT ALL PRIVILEGES ON karen_local.* TO 'pvtouch'@'localhost';
FLUSH PRIVILEGES;
```

### Tabla no existe
```sql
-- Ejecutar script de creación
USE karen_local;
SOURCE Scripts/crear_base_datos.sql;
```