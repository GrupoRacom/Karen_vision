using KarenVision.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KarenVision.Services
{
    /// <summary>
    /// Interfaz para el servicio de pedidos
    /// Define las operaciones disponibles para la gestión de pedidos
    /// </summary>
    public interface IPedidoService
    {
        /// <summary>
        /// Crea un nuevo pedido
        /// </summary>
        /// <param name="clienteId">ID del cliente</param>
        /// <param name="detalles">Lista de productos y cantidades</param>
        /// <param name="observaciones">Observaciones adicionales</param>
        /// <returns>El pedido creado</returns>
        Task<Pedido> CrearPedidoAsync(int clienteId, List<DetallePedido> detalles, string? observaciones = null);

        /// <summary>
        /// Obtiene un pedido por su ID
        /// </summary>
        /// <param name="pedidoId">ID del pedido</param>
        /// <returns>El pedido con sus detalles</returns>
        Task<Pedido?> ObtenerPedidoPorIdAsync(int pedidoId);

        /// <summary>
        /// Obtiene todos los pedidos de un cliente
        /// </summary>
        /// <param name="clienteId">ID del cliente</param>
        /// <returns>Lista de pedidos del cliente</returns>
        Task<IEnumerable<Pedido>> ObtenerPedidosPorClienteAsync(int clienteId);

        /// <summary>
        /// Actualiza el estado de un pedido
        /// </summary>
        /// <param name="pedidoId">ID del pedido</param>
        /// <param name="nuevoEstado">Nuevo estado del pedido</param>
        /// <returns>True si la actualización fue exitosa</returns>
        Task<bool> ActualizarEstadoPedidoAsync(int pedidoId, string nuevoEstado);

        /// <summary>
        /// Cancela un pedido y restaura el stock
        /// </summary>
        /// <param name="pedidoId">ID del pedido a cancelar</param>
        /// <returns>True si la cancelación fue exitosa</returns>
        Task<bool> CancelarPedidoAsync(int pedidoId);
    }
}