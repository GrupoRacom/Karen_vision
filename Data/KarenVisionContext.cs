using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using KarenVision.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace KarenVision.Data
{
    /// <summary>
    /// Contexto de base de datos para la aplicación Karen Vision
    /// Maneja la conexión a MySQL y las operaciones de Entity Framework
    /// </summary>
    public class KarenVisionContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<KarenVisionContext>? _logger;

        /// <summary>
        /// Constructor del contexto con configuración
        /// </summary>
        /// <param name="configuration">Configuración de la aplicación</param>
        /// <param name="logger">Logger opcional para debugging</param>
        public KarenVisionContext(IConfiguration configuration, ILogger<KarenVisionContext>? logger = null)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Constructor sin parámetros para migrations
        /// </summary>
        public KarenVisionContext()
        {
            _configuration = null!;
        }

        /// <summary>
        /// DbSet para la tabla de productos
        /// </summary>
        public DbSet<Producto> Productos { get; set; } = null!;

        /// <summary>
        /// DbSet para la tabla de clientes
        /// </summary>
        public DbSet<Cliente> Clientes { get; set; } = null!;

        /// <summary>
        /// DbSet para la tabla de pedidos
        /// </summary>
        public DbSet<Pedido> Pedidos { get; set; } = null!;

        /// <summary>
        /// DbSet para la tabla de detalles de pedido
        /// </summary>
        public DbSet<DetallePedido> DetallesPedido { get; set; } = null!;

        /// <summary>
        /// Configuración de la conexión a la base de datos
        /// </summary>
        /// <param name="optionsBuilder">Constructor de opciones de Entity Framework</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                try
                {
                    // Obtener cadena de conexión desde configuración
                    string? connectionString = _configuration?.GetConnectionString("DefaultConnection");
                    
                    // Si no hay configuración, usar cadena por defecto (para migrations)
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        connectionString = "Server=localhost;Database=KarenVisionDB;Uid=root;Pwd=password;SslMode=none;";
                        _logger?.LogWarning("Usando cadena de conexión por defecto. Considere configurar appsettings.json");
                    }

                    // Configurar MySQL con Pomelo
                    optionsBuilder.UseMySql(connectionString, 
                        new MySqlServerVersion(new Version(8, 0, 21)),
                        options => 
                        {
                            options.EnableRetryOnFailure(
                                maxRetryCount: 3,
                                maxRetryDelay: TimeSpan.FromSeconds(5),
                                errorNumbersToAdd: null);
                        });

                    // Habilitar logging sensible solo en desarrollo
                    if (_logger != null)
                    {
                        optionsBuilder.EnableSensitiveDataLogging();
                        optionsBuilder.LogTo(message => _logger.LogDebug(message));
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error configurando la conexión a base de datos");
                    throw;
                }
            }
        }

        /// <summary>
        /// Configuración del modelo de datos
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            try
            {
                // Configuración de la entidad Cliente
                modelBuilder.Entity<Cliente>(entity =>
                {
                    entity.HasIndex(e => e.IdIdentificacion)
                          .IsUnique()
                          .HasDatabaseName("IX_Clientes_IdIdentificacion");
                });

                // Configuración de la entidad Producto
                modelBuilder.Entity<Producto>(entity =>
                {
                    entity.Property(e => e.Precio)
                          .HasPrecision(18, 2);
                });

                // Configuración de la entidad Pedido
                modelBuilder.Entity<Pedido>(entity =>
                {
                    entity.Property(e => e.Total)
                          .HasPrecision(18, 2);
                    
                    // Relación con Cliente
                    entity.HasOne(d => d.Cliente)
                          .WithMany()
                          .HasForeignKey(d => d.ClienteId)
                          .OnDelete(DeleteBehavior.Restrict);
                });

                // Configuración de la entidad DetallePedido
                modelBuilder.Entity<DetallePedido>(entity =>
                {
                    entity.Property(e => e.PrecioUnitario)
                          .HasPrecision(18, 2);
                    
                    entity.Property(e => e.Subtotal)
                          .HasPrecision(18, 2);

                    // Relación con Pedido
                    entity.HasOne(d => d.Pedido)
                          .WithMany(p => p.Detalles)
                          .HasForeignKey(d => d.PedidoId)
                          .OnDelete(DeleteBehavior.Cascade);

                    // Relación con Producto
                    entity.HasOne(d => d.Producto)
                          .WithMany()
                          .HasForeignKey(d => d.ProductoId)
                          .OnDelete(DeleteBehavior.Restrict);
                });

                _logger?.LogDebug("Configuración del modelo completada exitosamente");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error configurando el modelo de datos");
                throw;
            }
        }

        /// <summary>
        /// Sobrescribir SaveChanges para agregar lógica adicional
        /// </summary>
        /// <returns>Número de entidades afectadas</returns>
        public override int SaveChanges()
        {
            try
            {
                // Calcular subtotales antes de guardar
                CalcularSubtotales();
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error guardando cambios en la base de datos");
                throw;
            }
        }

        /// <summary>
        /// Versión asíncrona de SaveChanges
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Número de entidades afectadas</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // Calcular subtotales antes de guardar
                CalcularSubtotales();
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error guardando cambios en la base de datos (async)");
                throw;
            }
        }

        /// <summary>
        /// Calcula los subtotales de los detalles de pedido
        /// </summary>
        private void CalcularSubtotales()
        {
            var detallesPedido = ChangeTracker.Entries<DetallePedido>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            foreach (var detalle in detallesPedido)
            {
                detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
            }
        }
    }
}