using KarenVision.Data;
using KarenVision.Models;
using KarenVision.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KarenVision.Services
{
    /// <summary>
    /// Servicio para la gestión de clientes
    /// Implementa la lógica de negocio relacionada con clientes
    /// </summary>
    public class ClienteService : IClienteService
    {
        private readonly KarenVisionContext _context;
        private readonly ILogger<ClienteService> _logger;

        /// <summary>
        /// Constructor del servicio de clientes
        /// </summary>
        /// <param name="context">Contexto de base de datos</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public ClienteService(KarenVisionContext context, ILogger<ClienteService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Busca un cliente por su ID de identificación (cédula, pasaporte, etc.)
        /// </summary>
        /// <param name="idIdentificacion">ID de identificación del cliente</param>
        /// <returns>El cliente si existe, null en caso contrario</returns>
        public async Task<Cliente?> BuscarClientePorIdIdentificacionAsync(string idIdentificacion)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(idIdentificacion))
                {
                    _logger.LogWarning("Se intentó buscar cliente con ID de identificación vacío");
                    return null;
                }

                _logger.LogDebug("Buscando cliente con ID identificación: {IdIdentificacion}", idIdentificacion);

                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.IdIdentificacion == idIdentificacion.Trim());

                if (cliente != null)
                {
                    _logger.LogDebug("Cliente encontrado: {NombreCompleto}", cliente.NombreCompleto);
                }
                else
                {
                    _logger.LogDebug("No se encontró cliente con ID identificación: {IdIdentificacion}", idIdentificacion);
                }

                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar cliente por ID identificación: {IdIdentificacion}", idIdentificacion);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo cliente después de validar que no exista uno con la misma identificación
        /// </summary>
        /// <param name="cliente">Datos del cliente a crear</param>
        /// <returns>El cliente creado con su ID asignado</returns>
        public async Task<Cliente> CrearClienteAsync(Cliente cliente)
        {
            try
            {
                if (cliente == null)
                {
                    throw new ArgumentNullException(nameof(cliente));
                }

                // Validar datos del cliente
                var erroresValidacion = ValidarCliente(cliente);
                if (erroresValidacion.Any())
                {
                    throw new ArgumentException($"Datos del cliente inválidos: {string.Join(", ", erroresValidacion)}");
                }

                _logger.LogInformation("Creando nuevo cliente: {NombreCompleto}, ID: {IdIdentificacion}", 
                    cliente.NombreCompleto, cliente.IdIdentificacion);

                // Verificar que no exista un cliente con la misma identificación
                var clienteExistente = await BuscarClientePorIdIdentificacionAsync(cliente.IdIdentificacion);
                if (clienteExistente != null)
                {
                    throw new InvalidOperationException($"Ya existe un cliente con ID de identificación: {cliente.IdIdentificacion}");
                }

                // Normalizar datos
                cliente.IdIdentificacion = cliente.IdIdentificacion.Trim();
                cliente.NombreCompleto = cliente.NombreCompleto.Trim();
                cliente.Email = string.IsNullOrWhiteSpace(cliente.Email) ? null : cliente.Email.Trim().ToLowerInvariant();
                cliente.Telefono = string.IsNullOrWhiteSpace(cliente.Telefono) ? null : cliente.Telefono.Trim();
                cliente.Direccion = string.IsNullOrWhiteSpace(cliente.Direccion) ? null : cliente.Direccion.Trim();
                cliente.FechaCreacion = DateTime.Now;

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Cliente creado exitosamente con ID: {ClienteId}", cliente.Id);
                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente: {NombreCompleto}", cliente?.NombreCompleto ?? "N/A");
                throw;
            }
        }

        /// <summary>
        /// Actualiza los datos de un cliente existente
        /// </summary>
        /// <param name="cliente">Cliente con los datos actualizados</param>
        /// <returns>True si la actualización fue exitosa</returns>
        public async Task<bool> ActualizarClienteAsync(Cliente cliente)
        {
            try
            {
                if (cliente == null)
                {
                    throw new ArgumentNullException(nameof(cliente));
                }

                _logger.LogDebug("Actualizando cliente ID: {ClienteId}", cliente.Id);

                // Validar datos del cliente
                var erroresValidacion = ValidarCliente(cliente);
                if (erroresValidacion.Any())
                {
                    throw new ArgumentException($"Datos del cliente inválidos: {string.Join(", ", erroresValidacion)}");
                }

                var clienteExistente = await _context.Clientes.FindAsync(cliente.Id);
                if (clienteExistente == null)
                {
                    _logger.LogWarning("No se encontró cliente con ID: {ClienteId}", cliente.Id);
                    return false;
                }

                // Verificar que no exista otro cliente con la misma identificación
                if (clienteExistente.IdIdentificacion != cliente.IdIdentificacion)
                {
                    var otroCliente = await BuscarClientePorIdIdentificacionAsync(cliente.IdIdentificacion);
                    if (otroCliente != null && otroCliente.Id != cliente.Id)
                    {
                        throw new InvalidOperationException($"Ya existe otro cliente con ID de identificación: {cliente.IdIdentificacion}");
                    }
                }

                // Actualizar campos
                clienteExistente.IdIdentificacion = cliente.IdIdentificacion.Trim();
                clienteExistente.NombreCompleto = cliente.NombreCompleto.Trim();
                clienteExistente.Email = string.IsNullOrWhiteSpace(cliente.Email) ? null : cliente.Email.Trim().ToLowerInvariant();
                clienteExistente.Telefono = string.IsNullOrWhiteSpace(cliente.Telefono) ? null : cliente.Telefono.Trim();
                clienteExistente.Direccion = string.IsNullOrWhiteSpace(cliente.Direccion) ? null : cliente.Direccion.Trim();

                await _context.SaveChangesAsync();

                _logger.LogInformation("Cliente {ClienteId} actualizado exitosamente", cliente.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cliente ID: {ClienteId}", cliente?.Id ?? 0);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un cliente por su ID interno
        /// </summary>
        /// <param name="id">ID interno del cliente</param>
        /// <returns>El cliente si existe, null en caso contrario</returns>
        public async Task<Cliente?> ObtenerClientePorIdAsync(int id)
        {
            try
            {
                _logger.LogDebug("Buscando cliente con ID: {ClienteId}", id);

                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    _logger.LogWarning("No se encontró cliente con ID: {ClienteId}", id);
                }

                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar cliente con ID: {ClienteId}", id);
                throw;
            }
        }

        /// <summary>
        /// Busca clientes por nombre usando coincidencia parcial
        /// </summary>
        /// <param name="nombre">Término de búsqueda</param>
        /// <returns>Lista de clientes que contienen el término en su nombre</returns>
        public async Task<IEnumerable<Cliente>> BuscarClientesPorNombreAsync(string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    _logger.LogDebug("Término de búsqueda vacío para clientes");
                    return new List<Cliente>();
                }

                _logger.LogDebug("Buscando clientes por nombre: {Nombre}", nombre);

                var clientes = await _context.Clientes
                    .Where(c => c.NombreCompleto.Contains(nombre.Trim()))
                    .OrderBy(c => c.NombreCompleto)
                    .ToListAsync();

                _logger.LogInformation("Se encontraron {Count} clientes con el término '{Nombre}'", 
                    clientes.Count, nombre);

                return clientes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar clientes por nombre: {Nombre}", nombre);
                throw;
            }
        }

        /// <summary>
        /// Valida que los datos del cliente cumplan con los requisitos de negocio
        /// </summary>
        /// <param name="cliente">Cliente a validar</param>
        /// <returns>Lista de errores de validación, vacía si es válido</returns>
        public List<string> ValidarCliente(Cliente cliente)
        {
            var errores = new List<string>();

            try
            {
                if (cliente == null)
                {
                    errores.Add("El cliente no puede ser nulo");
                    return errores;
                }

                // Validar ID de identificación
                if (string.IsNullOrWhiteSpace(cliente.IdIdentificacion))
                {
                    errores.Add("El ID de identificación es obligatorio");
                }
                else if (cliente.IdIdentificacion.Trim().Length < 3)
                {
                    errores.Add("El ID de identificación debe tener al menos 3 caracteres");
                }
                else if (cliente.IdIdentificacion.Trim().Length > 50)
                {
                    errores.Add("El ID de identificación no puede tener más de 50 caracteres");
                }

                // Validar nombre completo
                if (string.IsNullOrWhiteSpace(cliente.NombreCompleto))
                {
                    errores.Add("El nombre completo es obligatorio");
                }
                else if (cliente.NombreCompleto.Trim().Length < 2)
                {
                    errores.Add("El nombre completo debe tener al menos 2 caracteres");
                }
                else if (cliente.NombreCompleto.Trim().Length > 255)
                {
                    errores.Add("El nombre completo no puede tener más de 255 caracteres");
                }

                // Validar email si se proporciona
                if (!string.IsNullOrWhiteSpace(cliente.Email))
                {
                    if (cliente.Email.Length > 255)
                    {
                        errores.Add("El email no puede tener más de 255 caracteres");
                    }
                    else if (!EsEmailValido(cliente.Email))
                    {
                        errores.Add("El formato del email no es válido");
                    }
                }

                // Validar teléfono si se proporciona
                if (!string.IsNullOrWhiteSpace(cliente.Telefono))
                {
                    if (cliente.Telefono.Length > 20)
                    {
                        errores.Add("El teléfono no puede tener más de 20 caracteres");
                    }
                }

                _logger.LogDebug("Validación de cliente completada. Errores encontrados: {Count}", errores.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la validación del cliente");
                errores.Add("Error interno durante la validación");
            }

            return errores;
        }

        /// <summary>
        /// Valida el formato de un email usando expresión regular
        /// </summary>
        /// <param name="email">Email a validar</param>
        /// <returns>True si el formato es válido</returns>
        private static bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Expresión regular básica para validar email
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
    }
}