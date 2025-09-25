-- Datos de ejemplo para la base de datos karen_local
-- Ejecutar después de crear las tablas

USE karen_local;

-- Limpiar datos existentes (opcional)
SET FOREIGN_KEY_CHECKS = 0;
DELETE FROM detalles_pedido;
DELETE FROM pedidos;
DELETE FROM productos;
DELETE FROM clientes;
SET FOREIGN_KEY_CHECKS = 1;

-- Insertar clientes de ejemplo
INSERT INTO clientes (id_identificacion, nombre_completo, email, telefono, direccion) VALUES
('12345678', 'Juan Carlos Pérez', 'juan.perez@email.com', '555-0101', 'Av. Principal 123, Ciudad'),
('87654321', 'María Elena González', 'maria.gonzalez@email.com', '555-0102', 'Calle Secundaria 456, Ciudad'),
('11223344', 'Carlos Alberto Rodríguez', 'carlos.rodriguez@email.com', '555-0103', 'Plaza Central 789, Ciudad'),
('44332211', 'Ana Patricia López', 'ana.lopez@email.com', '555-0104', 'Barrio Norte 321, Ciudad'),
('55667788', 'Roberto Miguel Fernández', 'roberto.fernandez@email.com', '555-0105', 'Zona Sur 654, Ciudad');

-- Insertar productos de ejemplo
INSERT INTO productos (nombre, descripcion, precio, stock, activo) VALUES
('Laptop Dell Inspiron 15', 'Laptop Dell con procesador Intel Core i5, 8GB RAM, 256GB SSD', 849.99, 10, TRUE),
('Mouse Inalámbrico Logitech', 'Mouse inalámbrico con tecnología Bluetooth, ergonómico', 29.99, 50, TRUE),
('Teclado Mecánico Gaming', 'Teclado mecánico con retroiluminación RGB para gaming', 89.99, 25, TRUE),
('Monitor Samsung 24"', 'Monitor Full HD de 24 pulgadas con tecnología LED', 199.99, 15, TRUE),
('Auriculares Sony WH-1000XM4', 'Auriculares inalámbricos con cancelación de ruido', 299.99, 8, TRUE),
('Webcam Logitech C920', 'Webcam HD 1080p para videoconferencias', 79.99, 30, TRUE),
('Disco Duro Externo 1TB', 'Disco duro portátil USB 3.0 de 1TB', 69.99, 40, TRUE),
('Impresora HP LaserJet', 'Impresora láser monocromática para oficina', 159.99, 12, TRUE),
('Tablet Samsung Galaxy Tab', 'Tablet Android de 10 pulgadas con 64GB de almacenamiento', 249.99, 20, TRUE),
('Altavoces Bluetooth JBL', 'Altavoces portátiles con conectividad Bluetooth', 99.99, 18, TRUE);

-- Insertar algunos pedidos de ejemplo
INSERT INTO pedidos (cliente_id, total, estado, observaciones) VALUES
(1, 939.98, 'Completado', 'Pedido entregado satisfactoriamente'),
(2, 329.97, 'Pendiente', 'Cliente solicita entrega mañana'),
(3, 79.99, 'En proceso', 'Preparando para envío'),
(4, 549.98, 'Completado', 'Entrega realizada'),
(5, 199.99, 'Pendiente', 'Verificar disponibilidad');

-- Insertar detalles de pedidos de ejemplo
INSERT INTO detalles_pedido (pedido_id, producto_id, cantidad, precio_unitario, subtotal) VALUES
-- Pedido 1: Juan Carlos Pérez
(1, 1, 1, 849.99, 849.99),
(1, 2, 3, 29.99, 89.97),

-- Pedido 2: María Elena González
(2, 4, 1, 199.99, 199.99),
(2, 6, 1, 79.99, 79.99),
(2, 10, 1, 99.99, 99.99),

-- Pedido 3: Carlos Alberto Rodríguez
(3, 6, 1, 79.99, 79.99),

-- Pedido 4: Ana Patricia López  
(4, 5, 1, 299.99, 299.99),
(4, 9, 1, 249.99, 249.99),

-- Pedido 5: Roberto Miguel Fernández
(5, 4, 1, 199.99, 199.99);

-- Mostrar resumen de datos insertados
SELECT 'CLIENTES' as tabla, COUNT(*) as registros FROM clientes
UNION ALL
SELECT 'PRODUCTOS' as tabla, COUNT(*) as registros FROM productos
UNION ALL
SELECT 'PEDIDOS' as tabla, COUNT(*) as registros FROM pedidos
UNION ALL
SELECT 'DETALLES_PEDIDO' as tabla, COUNT(*) as registros FROM detalles_pedido;

SELECT 'Configuración completada exitosamente para karen_local' as mensaje;