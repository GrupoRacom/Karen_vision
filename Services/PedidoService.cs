using KarenVision.Data;
using KarenVision.Models;
using KarenVision.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KarenVision.Services
{
    /// <summary>
    /// Servicio para la gestión de pedidos
    /// Implementa la lógica de negocio relacionada con pedidos
    /// </summary>
    public class PedidoService : IPedidoService
    {
        private readonly KarenVisionContext _context;
        private readonly IProductoService _productoService;
        private readonly ILogger<PedidoService> _logger;

        /// <summary>
        /// Constructor del servicio de pedidos
        /// </summary>
        /// <param name="context">Contexto de base de datos</param>
        /// <param name="productoService">Servicio de productos</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public PedidoService(KarenVisionContext context, IProductoService productoService, ILogger<PedidoService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _productoService = productoService ?? throw new ArgumentNullException(nameof(productoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Crea un nuevo pedido con validación de stock y cálculo automático de totales
        /// </summary>
        /// <param name="clienteId">ID del cliente que realiza el pedido</param>
        /// <param name="detalles">Lista de detalles del pedido (productos y cantidades)</param>
        /// <param name="observaciones">Observaciones adicionales del pedido</param>
        /// <returns>El pedido creado</returns>
        public async Task<Pedido> CrearPedidoAsync(int clienteId, List<DetallePedido> detalles, string? observaciones = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                _logger.LogInformation("Iniciando creación de pedido para cliente {ClienteId}", clienteId);

                // Validar que el cliente existe
                var cliente = await _context.Clientes.FindAsync(clienteId);
                if (cliente == null)
                {
                    throw new ArgumentException($"Cliente con ID {clienteId} no encontrado", nameof(clienteId));
                }

                // Validar detalles del pedido
                if (detalles == null || !detalles.Any())
                {
                    throw new ArgumentException("El pedido debe tener al menos un producto", nameof(detalles));
                }

                // Verificar stock y obtener precios actuales
                decimal total = 0;
                foreach (var detalle in detalles)
                {
                    var producto = await _productoService.ObtenerProductoPorIdAsync(detalle.ProductoId);
                    if (producto == null)
                    {
                        throw new ArgumentException($"Producto con ID {detalle.ProductoId} no encontrado o no activo");
                    }

                    // Verificar stock disponible
                    if (!await _productoService.VerificarStockAsync(detalle.ProductoId, detalle.Cantidad))
                    {
                        throw new InvalidOperationException($"Stock insuficiente para el producto {producto.Nombre}");
                    }

                    // Actualizar precios con los actuales
                    detalle.PrecioUnitario = producto.Precio;
                    detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                    total += detalle.Subtotal;
                }

                // Crear el pedido
                var pedido = new Pedido
                {
                    ClienteId = clienteId,
                    FechaPedido = DateTime.Now,
                    Total = total,
                    Estado = "Pendiente",
                    Observaciones = observaciones,
                    Detalles = detalles
                };

                // Actualizar stock de productos
                foreach (var detalle in detalles)
                {
                    if (!await _productoService.ActualizarStockAsync(detalle.ProductoId, detalle.Cantidad))
                    {
                        throw new InvalidOperationException($"Error al actualizar stock del producto ID: {detalle.ProductoId}");
                    }
                }

                // Guardar el pedido
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                // Actualizar fecha del último pedido del cliente
                cliente.UltimoPedido = DateTime.Now;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                _logger.LogInformation("Pedido {PedidoId} creado exitosamente para cliente {ClienteId} con total {Total:C}", 
                    pedido.Id, clienteId, total);

                return pedido;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error al crear pedido para cliente {ClienteId}", clienteId);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un pedido por su ID incluyendo todos los detalles relacionados
        /// </summary>
        /// <param name="pedidoId">ID del pedido a buscar</param>
        /// <returns>El pedido con sus detalles o null si no existe</returns>
        public async Task<Pedido?> ObtenerPedidoPorIdAsync(int pedidoId)
        {
            try
            {
                _logger.LogDebug("Buscando pedido con ID: {PedidoId}", pedidoId);

                var pedido = await _context.Pedidos
                    .Include(p => p.Cliente)
                    .Include(p => p.Detalles)
                        .ThenInclude(d => d.Producto)
                    .FirstOrDefaultAsync(p => p.Id == pedidoId);

                if (pedido == null)
                {
                    _logger.LogWarning("No se encontró pedido con ID: {PedidoId}", pedidoId);
                }

                return pedido;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar pedido con ID: {PedidoId}", pedidoId);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los pedidos realizados por un cliente específico
        /// </summary>
        /// <param name="clienteId">ID del cliente</param>
        /// <returns>Lista de pedidos del cliente ordenados por fecha descendente</returns>
        public async Task<IEnumerable<Pedido>> ObtenerPedidosPorClienteAsync(int clienteId)
        {
            try
            {
                _logger.LogDebug("Obteniendo pedidos para cliente {ClienteId}", clienteId);

                var pedidos = await _context.Pedidos
                    .Include(p => p.Detalles)
                        .ThenInclude(d => d.Producto)
                    .Where(p => p.ClienteId == clienteId)
                    .OrderByDescending(p => p.FechaPedido)
                    .ToListAsync();

                _logger.LogInformation("Se encontraron {Count} pedidos para cliente {ClienteId}", 
                    pedidos.Count, clienteId);

                return pedidos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pedidos para cliente {ClienteId}", clienteId);
                throw;
            }
        }

        /// <summary>
        /// Actualiza el estado de un pedido
        /// </summary>
        /// <param name="pedidoId">ID del pedido</param>
        /// <param name="nuevoEstado">Nuevo estado (Pendiente, Procesando, Completado, Cancelado)</param>
        /// <returns>True si la actualización fue exitosa</returns>
        public async Task<bool> ActualizarEstadoPedidoAsync(int pedidoId, string nuevoEstado)
        {
            try
            {
                _logger.LogDebug("Actualizando estado de pedido {PedidoId} a {NuevoEstado}", pedidoId, nuevoEstado);

                var pedido = await _context.Pedidos.FindAsync(pedidoId);
                if (pedido == null)
                {
                    _logger.LogWarning("No se encontró pedido con ID: {PedidoId}", pedidoId);
                    return false;
                }

                string estadoAnterior = pedido.Estado;
                pedido.Estado = nuevoEstado;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Estado de pedido {PedidoId} actualizado de {EstadoAnterior} a {NuevoEstado}", 
                    pedidoId, estadoAnterior, nuevoEstado);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar estado de pedido {PedidoId}", pedidoId);
                throw;
            }
        }

        /// <summary>
        /// Cancela un pedido y restaura el stock de los productos
        /// </summary>
        /// <param name="pedidoId">ID del pedido a cancelar</param>
        /// <returns>True si la cancelación fue exitosa</returns>
        public async Task<bool> CancelarPedidoAsync(int pedidoId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                _logger.LogInformation("Iniciando cancelación de pedido {PedidoId}", pedidoId);

                var pedido = await _context.Pedidos
                    .Include(p => p.Detalles)
                    .FirstOrDefaultAsync(p => p.Id == pedidoId);

                if (pedido == null)
                {
                    _logger.LogWarning("No se encontró pedido con ID: {PedidoId}", pedidoId);
                    return false;
                }

                if (pedido.Estado == "Cancelado")
                {
                    _logger.LogWarning("El pedido {PedidoId} ya está cancelado", pedidoId);
                    return true; // Ya está cancelado
                }

                if (pedido.Estado == "Completado")
                {
                    _logger.LogWarning("No se puede cancelar el pedido {PedidoId} porque ya está completado", pedidoId);
                    return false;
                }

                // Restaurar stock de productos
                foreach (var detalle in pedido.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                    if (producto != null)
                    {
                        producto.Stock += detalle.Cantidad;
                        _logger.LogDebug("Stock restaurado para producto {ProductoId}: +{Cantidad}", 
                            detalle.ProductoId, detalle.Cantidad);
                    }
                }

                // Actualizar estado del pedido
                pedido.Estado = "Cancelado";
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                _logger.LogInformation("Pedido {PedidoId} cancelado exitosamente", pedidoId);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error al cancelar pedido {PedidoId}", pedidoId);
                throw;
            }
        }
    }
}