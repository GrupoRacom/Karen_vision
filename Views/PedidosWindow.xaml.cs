using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using KarenVision.Models;
using KarenVision.Services;

namespace KarenVision.Views
{
    /// <summary>
    /// Ventana del sistema de pedidos de Karen Vision
    /// Maneja la selección de productos, datos del cliente y creación de pedidos
    /// </summary>
    public partial class PedidosWindow : Window, INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PedidosWindow> _logger;
        private readonly IProductoService _productoService;
        private readonly IClienteService _clienteService;
        private readonly IPedidoService _pedidoService;

        private ObservableCollection<Producto> _productos = new();
        private ObservableCollection<DetallePedido> _carrito = new();
        private Cliente? _clienteActual;
        private decimal _total = 0;

        /// <summary>
        /// Evento para notificar cambios en las propiedades
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Total del pedido actual
        /// </summary>
        public decimal Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        /// <summary>
        /// Constructor de la ventana de pedidos
        /// </summary>
        /// <param name="serviceProvider">Proveedor de servicios</param>
        /// <param name="configuration">Configuración de la aplicación</param>
        public PedidosWindow(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            InitializeComponent();
            
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            
            // Crear logger básico
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<PedidosWindow>();

            // Obtener servicios
            _productoService = _serviceProvider.GetRequiredService<IProductoService>();
            _clienteService = _serviceProvider.GetRequiredService<IClienteService>();
            _pedidoService = _serviceProvider.GetRequiredService<IPedidoService>();

            _logger.LogInformation("Ventana de pedidos inicializada");

            // Configurar data binding
            DataContext = this;
            GridProductos.ItemsSource = _productos;
            GridCarrito.ItemsSource = _carrito;

            // Cargar productos iniciales
            _ = CargarProductosAsync();

            // Configurar placeholder para búsqueda
            ConfigurarPlaceholderBusqueda();
        }

        /// <summary>
        /// Constructor de la ventana de pedidos con logger
        /// </summary>
        /// <param name="serviceProvider">Proveedor de servicios</param>
        /// <param name="configuration">Configuración de la aplicación</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public PedidosWindow(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<PedidosWindow> logger)
        {
            InitializeComponent();
            
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Obtener servicios
            _productoService = _serviceProvider.GetRequiredService<IProductoService>();
            _clienteService = _serviceProvider.GetRequiredService<IClienteService>();
            _pedidoService = _serviceProvider.GetRequiredService<IPedidoService>();

            _logger.LogInformation("Ventana de pedidos inicializada");

            // Configurar data binding
            DataContext = this;
            GridProductos.ItemsSource = _productos;
            GridCarrito.ItemsSource = _carrito;

            // Cargar productos iniciales
            _ = CargarProductosAsync();

            // Configurar placeholder para búsqueda
            ConfigurarPlaceholderBusqueda();
        }

        /// <summary>
        /// Configura el placeholder para el campo de búsqueda
        /// </summary>
        private void ConfigurarPlaceholderBusqueda()
        {
            TxtBuscarProducto.GotFocus += (s, e) =>
            {
                if (TxtBuscarProducto.Text == "Buscar productos...")
                {
                    TxtBuscarProducto.Text = "";
                    TxtBuscarProducto.Foreground = System.Windows.Media.Brushes.Black;
                }
            };

            TxtBuscarProducto.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(TxtBuscarProducto.Text))
                {
                    TxtBuscarProducto.Text = "Buscar productos...";
                    TxtBuscarProducto.Foreground = System.Windows.Media.Brushes.Gray;
                }
            };

            // Establecer estado inicial
            TxtBuscarProducto.Text = "Buscar productos...";
            TxtBuscarProducto.Foreground = System.Windows.Media.Brushes.Gray;
        }

        /// <summary>
        /// Carga los productos desde la base de datos
        /// </summary>
        private async Task CargarProductosAsync()
        {
            try
            {
                _logger.LogDebug("Cargando productos desde base de datos");
                TxtEstadoConexion.Text = "Estado: Cargando productos...";

                var productos = await _productoService.ObtenerProductosActivosAsync();
                
                _productos.Clear();
                foreach (var producto in productos)
                {
                    _productos.Add(producto);
                }

                TxtEstadoConexion.Text = $"Estado: {_productos.Count} productos cargados";
                TxtEstadoConexion.Foreground = System.Windows.Media.Brushes.Green;

                _logger.LogInformation("Se cargaron {Count} productos", _productos.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar productos");
                TxtEstadoConexion.Text = "Estado: Error de conexión";
                TxtEstadoConexion.Foreground = System.Windows.Media.Brushes.Red;
                
                MessageBox.Show($"Error al cargar productos: {ex.Message}", 
                    "Error de Base de Datos", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Maneja la búsqueda de productos cuando se presiona una tecla
        /// </summary>
        private async void TxtBuscarProducto_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await BuscarProductosAsync();
            }
        }

        /// <summary>
        /// Maneja el clic del botón de búsqueda
        /// </summary>
        private async void BtnBuscarProducto_Click(object sender, RoutedEventArgs e)
        {
            await BuscarProductosAsync();
        }

        /// <summary>
        /// Realiza la búsqueda de productos
        /// </summary>
        private async Task BuscarProductosAsync()
        {
            try
            {
                string termino = TxtBuscarProducto.Text;
                if (termino == "Buscar productos..." || string.IsNullOrWhiteSpace(termino))
                {
                    await CargarProductosAsync();
                    return;
                }

                _logger.LogDebug("Buscando productos con término: {Termino}", termino);
                TxtEstadoConexion.Text = "Estado: Buscando...";

                var productos = await _productoService.BuscarProductosPorNombreAsync(termino);
                
                _productos.Clear();
                foreach (var producto in productos)
                {
                    _productos.Add(producto);
                }

                TxtEstadoConexion.Text = $"Estado: {_productos.Count} productos encontrados";
                TxtEstadoConexion.Foreground = System.Windows.Media.Brushes.Green;

                _logger.LogInformation("Búsqueda completada: {Count} productos encontrados", _productos.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar productos");
                MessageBox.Show($"Error al buscar productos: {ex.Message}", 
                    "Error de Búsqueda", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Agrega un producto al carrito
        /// </summary>
        private async void BtnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.Tag is Producto producto)
                {
                    _logger.LogDebug("Agregando producto al carrito: {ProductoNombre}", producto.Nombre);

                    // Verificar stock
                    if (!await _productoService.VerificarStockAsync(producto.Id, 1))
                    {
                        MessageBox.Show($"No hay stock suficiente del producto {producto.Nombre}", 
                            "Stock Insuficiente", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Verificar si el producto ya está en el carrito
                    var detalleExistente = _carrito.FirstOrDefault(d => d.ProductoId == producto.Id);
                    if (detalleExistente != null)
                    {
                        // Incrementar cantidad
                        if (await _productoService.VerificarStockAsync(producto.Id, detalleExistente.Cantidad + 1))
                        {
                            detalleExistente.Cantidad++;
                            CalcularSubtotal(detalleExistente);
                        }
                        else
                        {
                            MessageBox.Show($"No hay stock suficiente del producto {producto.Nombre}", 
                                "Stock Insuficiente", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        // Agregar nuevo producto
                        var nuevoDetalle = new DetallePedido
                        {
                            ProductoId = producto.Id,
                            Producto = producto,
                            Cantidad = 1,
                            PrecioUnitario = producto.Precio
                        };
                        CalcularSubtotal(nuevoDetalle);
                        _carrito.Add(nuevoDetalle);
                    }

                    ActualizarTotales();
                    _logger.LogDebug("Producto agregado al carrito exitosamente");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar producto al carrito");
                MessageBox.Show($"Error al agregar producto: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Elimina un producto del carrito
        /// </summary>
        private void BtnEliminarDelCarrito_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn && btn.Tag is DetallePedido detalle)
                {
                    _logger.LogDebug("Eliminando producto del carrito: {ProductoNombre}", detalle.Producto?.Nombre);
                    
                    _carrito.Remove(detalle);
                    ActualizarTotales();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto del carrito");
                MessageBox.Show($"Error al eliminar producto: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Maneja el cambio de cantidad en el carrito
        /// </summary>
        private async void TxtCantidad_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is TextBox txtBox && txtBox.DataContext is DetallePedido detalle)
                {
                    if (int.TryParse(txtBox.Text, out int nuevaCantidad) && nuevaCantidad > 0)
                    {
                        // Verificar stock
                        if (await _productoService.VerificarStockAsync(detalle.ProductoId, nuevaCantidad))
                        {
                            detalle.Cantidad = nuevaCantidad;
                            CalcularSubtotal(detalle);
                            ActualizarTotales();
                        }
                        else
                        {
                            MessageBox.Show($"No hay stock suficiente del producto {detalle.Producto?.Nombre}", 
                                "Stock Insuficiente", MessageBoxButton.OK, MessageBoxImage.Warning);
                            txtBox.Text = detalle.Cantidad.ToString();
                        }
                    }
                    else
                    {
                        txtBox.Text = detalle.Cantidad.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cantidad");
                MessageBox.Show($"Error al actualizar cantidad: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Busca un cliente por su ID de identificación
        /// </summary>
        private async void TxtIdIdentificacion_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string idIdentificacion = TxtIdIdentificacion.Text.Trim();
                if (!string.IsNullOrEmpty(idIdentificacion))
                {
                    _logger.LogDebug("Buscando cliente con ID: {IdIdentificacion}", idIdentificacion);
                    
                    var cliente = await _clienteService.BuscarClientePorIdIdentificacionAsync(idIdentificacion);
                    if (cliente != null)
                    {
                        _clienteActual = cliente;
                        TxtNombreCompleto.Text = cliente.NombreCompleto;
                        TxtEmail.Text = cliente.Email ?? "";
                        TxtTelefono.Text = cliente.Telefono ?? "";
                        
                        TxtBienvenida.Text = $"¡Bienvenido de nuevo, {cliente.NombreCompleto}!";
                        _logger.LogDebug("Cliente encontrado y cargado: {NombreCompleto}", cliente.NombreCompleto);
                    }
                    else
                    {
                        _clienteActual = null;
                        TxtBienvenida.Text = "Cliente nuevo - Complete todos los datos";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar cliente");
                MessageBox.Show($"Error al buscar cliente: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Abre una ventana para buscar clientes existentes
        /// </summary>
        private async void BtnBuscarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Esta funcionalidad se puede expandir con una ventana de búsqueda
                MessageBox.Show("Funcionalidad de búsqueda avanzada de clientes disponible en versión futura.", 
                    "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar clientes");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Limpia el carrito de compras
        /// </summary>
        private void BtnLimpiarCarrito_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_carrito.Count == 0)
                {
                    MessageBox.Show("El carrito ya está vacío.", 
                        "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show("¿Está seguro que desea limpiar el carrito?", 
                    "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _carrito.Clear();
                    ActualizarTotales();
                    _logger.LogDebug("Carrito limpiado por el usuario");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al limpiar carrito");
                MessageBox.Show($"Error al limpiar carrito: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Confirma y procesa el pedido
        /// </summary>
        private async void BtnConfirmarPedido_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validar carrito
                if (_carrito.Count == 0)
                {
                    MessageBox.Show("Agregue al menos un producto al carrito.", 
                        "Carrito Vacío", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Validar datos del cliente
                if (string.IsNullOrWhiteSpace(TxtIdIdentificacion.Text) || 
                    string.IsNullOrWhiteSpace(TxtNombreCompleto.Text))
                {
                    MessageBox.Show("Complete los datos del cliente (ID y Nombre Completo).", 
                        "Datos Incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _logger.LogInformation("Iniciando proceso de confirmación de pedido");

                // Crear o actualizar cliente
                var cliente = _clienteActual ?? new Cliente();
                cliente.IdIdentificacion = TxtIdIdentificacion.Text.Trim();
                cliente.NombreCompleto = TxtNombreCompleto.Text.Trim();
                cliente.Email = string.IsNullOrWhiteSpace(TxtEmail.Text) ? null : TxtEmail.Text.Trim();
                cliente.Telefono = string.IsNullOrWhiteSpace(TxtTelefono.Text) ? null : TxtTelefono.Text.Trim();

                // Validar cliente
                var erroresValidacion = _clienteService.ValidarCliente(cliente);
                if (erroresValidacion.Any())
                {
                    MessageBox.Show($"Errores en los datos del cliente:\n{string.Join("\n", erroresValidacion)}", 
                        "Datos Inválidos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Guardar cliente si es nuevo
                if (_clienteActual == null)
                {
                    cliente = await _clienteService.CrearClienteAsync(cliente);
                    _clienteActual = cliente;
                }
                else
                {
                    await _clienteService.ActualizarClienteAsync(cliente);
                }

                // Crear lista de detalles para el pedido
                var detallesPedido = _carrito.ToList();

                // Mostrar confirmación final
                var mensaje = $"Confirmar pedido:\n\n" +
                             $"Cliente: {cliente.NombreCompleto}\n" +
                             $"Total de items: {detallesPedido.Sum(d => d.Cantidad)}\n" +
                             $"Total a pagar: {Total:C}\n\n" +
                             "¿Desea confirmar este pedido?";

                var confirmacion = MessageBox.Show(mensaje, "Confirmar Pedido", 
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (confirmacion == MessageBoxResult.Yes)
                {
                    // Crear el pedido
                    var pedido = await _pedidoService.CrearPedidoAsync(
                        cliente.Id, detallesPedido, TxtObservaciones.Text);

                    MessageBox.Show($"¡Pedido creado exitosamente!\n\nNúmero de pedido: {pedido.Id}\nTotal: {pedido.Total:C}", 
                        "Pedido Confirmado", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Limpiar formulario
                    LimpiarFormulario();
                    await CargarProductosAsync(); // Recargar productos para actualizar stock

                    _logger.LogInformation("Pedido {PedidoId} creado exitosamente", pedido.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al confirmar pedido");
                MessageBox.Show($"Error al procesar el pedido: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Vuelve a la ventana principal
        /// </summary>
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_carrito.Count > 0)
                {
                    var result = MessageBox.Show("Tiene productos en el carrito. ¿Está seguro que desea salir?", 
                        "Confirmar Salida", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    
                    if (result != MessageBoxResult.Yes)
                        return;
                }

                _logger.LogInformation("Usuario regresando a ventana principal");
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al volver a ventana principal");
                Close();
            }
        }

        /// <summary>
        /// Calcula el subtotal de un detalle de pedido
        /// </summary>
        /// <param name="detalle">Detalle a calcular</param>
        private static void CalcularSubtotal(DetallePedido detalle)
        {
            detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
        }

        /// <summary>
        /// Actualiza los totales del carrito
        /// </summary>
        private void ActualizarTotales()
        {
            try
            {
                var totalItems = _carrito.Sum(d => d.Cantidad);
                Total = _carrito.Sum(d => d.Subtotal);

                TxtTotalItems.Text = $"Items: {totalItems}";
                TxtTotal.Text = $"Total: {Total:C}";

                // Forzar actualización de la interfaz
                GridCarrito.Items.Refresh();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar totales");
            }
        }

        /// <summary>
        /// Limpia el formulario después de confirmar un pedido
        /// </summary>
        private void LimpiarFormulario()
        {
            try
            {
                _carrito.Clear();
                _clienteActual = null;
                
                TxtIdIdentificacion.Text = "";
                TxtNombreCompleto.Text = "";
                TxtEmail.Text = "";
                TxtTelefono.Text = "";
                TxtObservaciones.Text = "";
                
                TxtBienvenida.Text = "Seleccione productos y complete los datos del cliente";
                ActualizarTotales();

                _logger.LogDebug("Formulario limpiado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al limpiar formulario");
            }
        }

        /// <summary>
        /// Notifica cambios en las propiedades para data binding
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad que cambió</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Maneja el cierre de la ventana
        /// </summary>
        /// <param name="e">Argumentos del evento de cierre</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                _logger.LogInformation("Cerrando ventana de pedidos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante cierre de ventana de pedidos");
            }

            base.OnClosing(e);
        }
    }
}