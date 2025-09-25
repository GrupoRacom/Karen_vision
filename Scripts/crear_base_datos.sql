-- Script para crear la base de datos Karen Vision
-- Compatible con MySQL 8.0+
-- Ejecutar con permisos de administrador
-- Base de datos: karen_local
-- Usuario: pvtouch
-- Password: familia

-- Crear base de datos
CREATE DATABASE IF NOT EXISTS karen_local 
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

USE karen_local;

-- Crear tabla de clientes
CREATE TABLE IF NOT EXISTS clientes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    id_identificacion VARCHAR(50) NOT NULL UNIQUE,
    nombre_completo VARCHAR(255) NOT NULL,
    email VARCHAR(255),
    telefono VARCHAR(20),
    direccion TEXT,
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    ultimo_pedido DATETIME,
    INDEX idx_id_identificacion (id_identificacion),
    INDEX idx_nombre_completo (nombre_completo)
) ENGINE=InnoDB;

-- Crear tabla de productos
CREATE TABLE IF NOT EXISTS productos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL,
    descripcion TEXT,
    precio DECIMAL(18,2) NOT NULL,
    stock INT DEFAULT 0,
    imagen_url VARCHAR(500),
    activo BOOLEAN DEFAULT TRUE,
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_nombre (nombre),
    INDEX idx_activo (activo),
    INDEX idx_precio (precio)
) ENGINE=InnoDB;

-- Crear tabla de pedidos
CREATE TABLE IF NOT EXISTS pedidos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    cliente_id INT NOT NULL,
    fecha_pedido DATETIME DEFAULT CURRENT_TIMESTAMP,
    total DECIMAL(18,2) NOT NULL,
    estado VARCHAR(50) DEFAULT 'Pendiente',
    observaciones TEXT,
    FOREIGN KEY (cliente_id) REFERENCES clientes(id) ON DELETE RESTRICT,
    INDEX idx_cliente_id (cliente_id),
    INDEX idx_fecha_pedido (fecha_pedido),
    INDEX idx_estado (estado)
) ENGINE=InnoDB;

-- Crear tabla de detalles de pedido
CREATE TABLE IF NOT EXISTS detalles_pedido (
    id INT AUTO_INCREMENT PRIMARY KEY,
    pedido_id INT NOT NULL,
    producto_id INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(18,2) NOT NULL,
    subtotal DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (pedido_id) REFERENCES pedidos(id) ON DELETE CASCADE,
    FOREIGN KEY (producto_id) REFERENCES productos(id) ON DELETE RESTRICT,
    INDEX idx_pedido_id (pedido_id),
    INDEX idx_producto_id (producto_id)
) ENGINE=InnoDB;

-- Insertar datos de ejemplo para productos
INSERT INTO productos (nombre, descripcion, precio, stock, activo) VALUES
('Laptop Dell Inspiron 15', 'Laptop Dell con procesador Intel Core i5, 8GB RAM, 256GB SSD', 849.99, 10, TRUE),
('Mouse Inalámbrico Logitech', 'Mouse inalámbrico con tecnología Bluetooth, ergonómico', 29.99, 50, TRUE),
('Teclado Mecánico Gaming', 'Teclado mecánico con retroiluminación RGB para gaming', 89.99, 25, TRUE),
('Monitor Samsung 24"', 'Monitor Full HD de 24 pulgadas con tecnología LED', 199.99, 15, TRUE),
('Auriculares Sony WH-1000XM4', 'Auriculares inalámbricos con cancelación de ruido', 299.99, 8, TRUE),
('Webcam Logitech C920', 'Webcam HD 1080p para videoconferencias', 79.99, 30, TRUE),
('Disco Duro Externo 1TB', 'Disco duro portátil USB 3.0 de 1TB', 69.99, 40, TRUE),
('Smartphone Samsung Galaxy', 'Smartphone con pantalla de 6.1", 128GB almacenamiento', 599.99, 12, TRUE),
('Tablet iPad Air', 'Tablet iPad Air con pantalla de 10.9" y chip A14', 499.99, 6, TRUE),
('Impresora HP LaserJet', 'Impresora láser monocromática con WiFi', 159.99, 18, TRUE),
('Cable USB-C', 'Cable USB-C de alta velocidad, 2 metros', 19.99, 100, TRUE),
('Adaptador HDMI', 'Adaptador USB-C a HDMI 4K', 24.99, 75, TRUE),
('Batería Portátil 10000mAh', 'Power bank con carga rápida y múltiples puertos', 39.99, 60, TRUE),
('Soporte para Laptop', 'Soporte ergonómico ajustable para laptop', 49.99, 35, TRUE),
('Router WiFi 6', 'Router inalámbrico de alta velocidad WiFi 6', 129.99, 20, TRUE);

-- Insertar datos de ejemplo para clientes
INSERT INTO clientes (id_identificacion, nombre_completo, email, telefono, direccion) VALUES
('12345678', 'María García López', 'maria.garcia@email.com', '555-0101', 'Calle Principal 123, Ciudad'),
('87654321', 'Juan Carlos Rodríguez', 'juan.rodriguez@email.com', '555-0102', 'Avenida Central 456, Ciudad'),
('11223344', 'Ana Sofía Martínez', 'ana.martinez@email.com', '555-0103', 'Calle Secundaria 789, Ciudad'),
('44332211', 'Pedro Luis Fernández', 'pedro.fernandez@email.com', '555-0104', 'Boulevard Norte 321, Ciudad'),
('55667788', 'Carmen Elena Vásquez', 'carmen.vasquez@email.com', '555-0105', 'Calle Sur 654, Ciudad');

-- Insertar pedidos de ejemplo
INSERT INTO pedidos (cliente_id, total, estado, observaciones) VALUES
(1, 879.98, 'Completado', 'Entrega rápida solicitada'),
(2, 129.98, 'Procesando', 'Cliente prefiere entrega en la mañana'),
(3, 599.99, 'Pendiente', 'Verificar disponibilidad de color'),
(1, 49.98, 'Completado', 'Segunda compra del cliente'),
(4, 289.97, 'Procesando', 'Cliente corporativo');

-- Insertar detalles de pedidos de ejemplo
INSERT INTO detalles_pedido (pedido_id, producto_id, cantidad, precio_unitario, subtotal) VALUES
-- Pedido 1 (María García)
(1, 1, 1, 849.99, 849.99),  -- Laptop Dell
(1, 2, 1, 29.99, 29.99),    -- Mouse Logitech
-- Pedido 2 (Juan Carlos)
(2, 3, 1, 89.99, 89.99),    -- Teclado Gaming
(2, 6, 1, 79.99, 79.99),    -- Webcam
(2, 11, 2, 19.99, 39.98),   -- Cable USB-C x2
-- Pedido 3 (Ana Sofía)
(3, 8, 1, 599.99, 599.99),  -- Smartphone Samsung
-- Pedido 4 (María García - segundo pedido)
(4, 13, 1, 39.99, 39.99),   -- Batería Portátil
(4, 12, 1, 24.99, 24.99),   -- Adaptador HDMI
-- Pedido 5 (Pedro Luis)
(5, 4, 1, 199.99, 199.99),  -- Monitor Samsung
(5, 5, 1, 299.99, 299.99),  -- Auriculares Sony
(5, 14, 2, 49.99, 99.98);   -- Soporte Laptop x2

-- Crear usuario para la aplicación (opcional - para mayor seguridad)
-- IMPORTANTE: Cambiar la contraseña por una más segura en producción
CREATE USER IF NOT EXISTS 'karenvision_app'@'localhost' IDENTIFIED BY 'KarenVision2024!';
CREATE USER IF NOT EXISTS 'karenvision_app'@'%' IDENTIFIED BY 'KarenVision2024!';

-- Otorgar permisos al usuario de la aplicación
GRANT SELECT, INSERT, UPDATE, DELETE ON KarenVisionDB.* TO 'karenvision_app'@'localhost';
GRANT SELECT, INSERT, UPDATE, DELETE ON KarenVisionDB.* TO 'karenvision_app'@'%';

-- Actualizar privilegios
FLUSH PRIVILEGES;

-- Mostrar resumen de la base de datos creada
SELECT 'Base de datos Karen Vision creada exitosamente!' as Mensaje;
SELECT 'Estadísticas de la base de datos:' as Info;
SELECT 
    (SELECT COUNT(*) FROM productos WHERE activo = TRUE) as Productos_Activos,
    (SELECT COUNT(*) FROM clientes) as Clientes_Registrados,
    (SELECT COUNT(*) FROM pedidos) as Pedidos_Totales,
    (SELECT SUM(total) FROM pedidos) as Ventas_Totales;

-- Mostrar algunos productos de ejemplo
SELECT 'Productos disponibles (muestra):' as Info;
SELECT nombre, precio, stock FROM productos WHERE activo = TRUE LIMIT 5;