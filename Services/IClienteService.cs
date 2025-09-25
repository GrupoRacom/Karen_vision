using KarenVision.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KarenVision.Services
{
    /// <summary>
    /// Interfaz para el servicio de clientes
    /// Define las operaciones disponibles para la gestión de clientes
    /// </summary>
    public interface IClienteService
    {
        /// <summary>
        /// Busca un cliente por su ID de identificación
        /// </summary>
        /// <param name="idIdentificacion">ID de identificación del cliente</param>
        /// <returns>El cliente si existe, null en caso contrario</returns>
        Task<Cliente?> BuscarClientePorIdIdentificacionAsync(string idIdentificacion);

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        /// <param name="cliente">Datos del cliente a crear</param>
        /// <returns>El cliente creado con su ID asignado</returns>
        Task<Cliente> CrearClienteAsync(Cliente cliente);

        /// <summary>
        /// Actualiza los datos de un cliente existente
        /// </summary>
        /// <param name="cliente">Cliente con los datos actualizados</param>
        /// <returns>True si la actualización fue exitosa</returns>
        Task<bool> ActualizarClienteAsync(Cliente cliente);

        /// <summary>
        /// Obtiene un cliente por su ID interno
        /// </summary>
        /// <param name="id">ID interno del cliente</param>
        /// <returns>El cliente si existe, null en caso contrario</returns>
        Task<Cliente?> ObtenerClientePorIdAsync(int id);

        /// <summary>
        /// Busca clientes por nombre
        /// </summary>
        /// <param name="nombre">Nombre o parte del nombre a buscar</param>
        /// <returns>Lista de clientes que coinciden con el criterio</returns>
        Task<IEnumerable<Cliente>> BuscarClientesPorNombreAsync(string nombre);

        /// <summary>
        /// Valida que los datos del cliente sean correctos
        /// </summary>
        /// <param name="cliente">Cliente a validar</param>
        /// <returns>Lista de errores de validación, vacía si es válido</returns>
        List<string> ValidarCliente(Cliente cliente);
    }
}