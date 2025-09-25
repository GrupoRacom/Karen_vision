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
    /// Servicio para la gestión de productos
    /// Implementa la lógica de negocio relacionada con productos
    /// </summary>
    public class ProductoService : IProductoService
    {
        private readonly KarenVisionContext _context;
        private readonly ILogger<ProductoService> _logger;

        /// <summary>
        /// Constructor del servicio de productos
        /// </summary>
        /// <param name="context">Contexto de base de datos</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public ProductoService(KarenVisionContext context, ILogger<ProductoService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene todos los productos activos de la base de datos
        /// </summary>
        /// <returns>Lista de productos activos ordenados por nombre</returns>
        public async Task<IEnumerable<Producto>> ObtenerProductosActivosAsync()
        {
            try
            {
                _logger.LogDebug("Obteniendo productos activos");
                
                var productos = await _context.Productos
                    .Where(p => p.Activo)
                    .OrderBy(p => p.Nombre)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} productos activos", productos.Count);
                return productos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos activos");
                throw;
            }
        }

        /// <summary>
        /// Obtiene un producto específico por su ID
        /// </summary>
        /// <param name="id">ID del producto a buscar</param>
        /// <returns>El producto si existe y está activo, null en caso contrario</returns>
        public async Task<Producto?> ObtenerProductoPorIdAsync(int id)
        {
            try
            {
                _logger.LogDebug("Buscando producto con ID: {ProductoId}", id);
                
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.Id == id && p.Activo);

                if (producto == null)
                {
                    _logger.LogWarning("No se encontró producto activo con ID: {ProductoId}", id);
                }
                else
                {
                    _logger.LogDebug("Producto encontrado: {ProductoNombre}", producto.Nombre);
                }

                return producto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar producto con ID: {ProductoId}", id);
                throw;
            }
        }

        /// <summary>
        /// Busca productos por nombre usando coincidencia parcial
        /// </summary>
        /// <param name="nombre">Término de búsqueda</param>
        /// <returns>Lista de productos que contienen el término en su nombre</returns>
        public async Task<IEnumerable<Producto>> BuscarProductosPorNombreAsync(string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    _logger.LogDebug("Término de búsqueda vacío, devolviendo todos los productos activos");
                    return await ObtenerProductosActivosAsync();
                }

                _logger.LogDebug("Buscando productos por nombre: {Nombre}", nombre);
                
                var productos = await _context.Productos
                    .Where(p => p.Activo && p.Nombre.Contains(nombre))
                    .OrderBy(p => p.Nombre)
                    .ToListAsync();

                _logger.LogInformation("Se encontraron {Count} productos con el término '{Nombre}'", 
                    productos.Count, nombre);
                
                return productos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar productos por nombre: {Nombre}", nombre);
                throw;
            }
        }

        /// <summary>
        /// Verifica si hay stock suficiente para satisfacer una cantidad requerida
        /// </summary>
        /// <param name="productoId">ID del producto</param>
        /// <param name="cantidad">Cantidad requerida</param>
        /// <returns>True si hay stock suficiente</returns>
        public async Task<bool> VerificarStockAsync(int productoId, int cantidad)
        {
            try
            {
                _logger.LogDebug("Verificando stock para producto {ProductoId}, cantidad: {Cantidad}", 
                    productoId, cantidad);
                
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.Id == productoId && p.Activo);

                if (producto == null)
                {
                    _logger.LogWarning("Producto no encontrado para verificación de stock: {ProductoId}", productoId);
                    return false;
                }

                bool stockSuficiente = producto.Stock >= cantidad;
                
                _logger.LogDebug("Stock actual: {Stock}, requerido: {Cantidad}, suficiente: {Suficiente}", 
                    producto.Stock, cantidad, stockSuficiente);

                return stockSuficiente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar stock para producto: {ProductoId}", productoId);
                throw;
            }
        }

        /// <summary>
        /// Actualiza el stock de un producto restando la cantidad especificada
        /// </summary>
        /// <param name="productoId">ID del producto</param>
        /// <param name="cantidadRestar">Cantidad a restar del stock</param>
        /// <returns>True si la actualización fue exitosa</returns>
        public async Task<bool> ActualizarStockAsync(int productoId, int cantidadRestar)
        {
            try
            {
                _logger.LogDebug("Actualizando stock para producto {ProductoId}, restar: {Cantidad}", 
                    productoId, cantidadRestar);
                
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.Id == productoId && p.Activo);

                if (producto == null)
                {
                    _logger.LogWarning("Producto no encontrado para actualización de stock: {ProductoId}", productoId);
                    return false;
                }

                if (producto.Stock < cantidadRestar)
                {
                    _logger.LogWarning("Stock insuficiente para producto {ProductoId}. Stock: {Stock}, Requerido: {Cantidad}", 
                        productoId, producto.Stock, cantidadRestar);
                    return false;
                }

                int stockAnterior = producto.Stock;
                producto.Stock -= cantidadRestar;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Stock actualizado para producto {ProductoId}: {StockAnterior} -> {StockNuevo}", 
                    productoId, stockAnterior, producto.Stock);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar stock para producto: {ProductoId}", productoId);
                throw;
            }
        }
    }
}