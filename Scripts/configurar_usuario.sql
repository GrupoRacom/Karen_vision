-- Script para configurar usuario de base de datos MySQL
-- Base de datos: karen_local
-- Usuario: pvtouch
-- Password: familia

-- EJECUTAR ESTE SCRIPT COMO ADMINISTRADOR DE MYSQL (root)

-- Crear el usuario pvtouch si no existe
CREATE USER IF NOT EXISTS 'pvtouch'@'localhost' IDENTIFIED BY 'familia';

-- Otorgar todos los permisos sobre la base de datos karen_local
GRANT ALL PRIVILEGES ON karen_local.* TO 'pvtouch'@'localhost';

-- También permitir acceso desde cualquier host (opcional, para desarrollo)
CREATE USER IF NOT EXISTS 'pvtouch'@'%' IDENTIFIED BY 'familia';
GRANT ALL PRIVILEGES ON karen_local.* TO 'pvtouch'@'%';

-- Aplicar los cambios
FLUSH PRIVILEGES;

-- Verificar que el usuario fue creado correctamente
SELECT User, Host FROM mysql.user WHERE User = 'pvtouch';

-- Mostrar permisos del usuario
SHOW GRANTS FOR 'pvtouch'@'localhost';