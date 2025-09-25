using KarenVision.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KarenVision.Services
{
    /// <summary>
    /// Interfaz para el servicio de productos
    /// Define las operaciones disponibles para la gestión de productos
    /// </summary>
    public interface IProductoService
    {
        /// <summary>
        /// Obtiene todos los productos activos
        /// </summary>
        /// <returns>Lista de productos activos</returns>
        Task<IEnumerable<Producto>> ObtenerProductosActivosAsync();

        /// <summary>
        /// Obtiene un producto por su ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>El producto si existe, null en caso contrario</returns>
        Task<Producto?> ObtenerProductoPorIdAsync(int id);

        /// <summary>
        /// Busca productos por nombre
        /// </summary>
        /// <param name="nombre">Nombre o parte del nombre a buscar</param>
        /// <returns>Lista de productos que coinciden con el criterio</returns>
        Task<IEnumerable<Producto>> BuscarProductosPorNombreAsync(string nombre);

        /// <summary>
        /// Verifica si hay stock suficiente de un producto
        /// </summary>
        /// <param name="productoId">ID del producto</param>
        /// <param name="cantidad">Cantidad requerida</param>
        /// <returns>True si hay stock suficiente</returns>
        Task<bool> VerificarStockAsync(int productoId, int cantidad);

        /// <summary>
        /// Actualiza el stock de un producto
        /// </summary>
        /// <param name="productoId">ID del producto</param>
        /// <param name="cantidadRestar">Cantidad a restar del stock</param>
        /// <returns>True si la actualización fue exitosa</returns>
        Task<bool> ActualizarStockAsync(int productoId, int cantidadRestar);
    }
}