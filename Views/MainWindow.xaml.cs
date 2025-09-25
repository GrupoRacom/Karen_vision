using System;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KarenVision.Views
{
    /// <summary>
    /// Ventana principal de la aplicación Karen Vision
    /// Maneja la lógica de navegación y control de acceso mediante publicidad
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MainWindow> _logger;
        private readonly IServiceProvider _serviceProvider;
        private bool _publicidadVista = false;

        /// <summary>
        /// Constructor de la ventana principal
        /// </summary>
        /// <param name="configuration">Configuración de la aplicación</param>
        /// <param name="logger">Logger para registro de eventos</param>
        /// <param name="serviceProvider">Proveedor de servicios para inyección de dependencias</param>
        public MainWindow(IConfiguration configuration, ILogger<MainWindow> logger, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            _logger.LogInformation("Ventana principal inicializada");
            
            // Verificar configuración de publicidad
            var requireInteraction = _configuration.GetValue<bool>("AppSettings:RequireAdvertisementInteraction", true);
            _logger.LogDebug("Interacción con publicidad requerida: {RequireInteraction}", requireInteraction);
        }

        /// <summary>
        /// Manejador del evento de clic en el botón "Ver Publicidad"
        /// Abre la ventana de publicidad
        /// </summary>
        /// <param name="sender">Objeto que disparó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void BtnVerPublicidad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _logger.LogInformation("Usuario iniciando visualización de publicidad");

                // Crear y mostrar ventana de publicidad
                var publicidadWindow = new PublicidadWindow(_configuration, _logger);
                
                // Suscribirse al evento de publicidad completada
                publicidadWindow.PublicidadCompletada += PublicidadWindow_PublicidadCompletada;
                
                // Mostrar ventana como modal
                bool? result = publicidadWindow.ShowDialog();
                
                if (result == true)
                {
                    _logger.LogDebug("Ventana de publicidad cerrada con resultado positivo");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al abrir ventana de publicidad");
                MessageBox.Show($"Error al mostrar la publicidad: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Manejador del evento de publicidad completada
        /// Habilita el acceso al sistema de pedidos
        /// </summary>
        /// <param name="sender">Objeto que disparó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void PublicidadWindow_PublicidadCompletada(object? sender, EventArgs e)
        {
            try
            {
                _logger.LogInformation("Publicidad completada, habilitando acceso a pedidos");

                _publicidadVista = true;

                // Actualizar interfaz de usuario
                Dispatcher.Invoke(() =>
                {
                    BtnAccederPedidos.IsEnabled = true;
                    BtnAccederPedidos.Style = (Style)FindResource("PrimaryButton");
                    TxtEstado.Text = "¡Publicidad vista! Ahora puede acceder al sistema de pedidos";
                    TxtEstado.Foreground = System.Windows.Media.Brushes.Green;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar publicidad completada");
                MessageBox.Show($"Error al procesar la publicidad: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Manejador del evento de clic en el botón "Acceder a Pedidos"
        /// Abre la ventana del sistema de pedidos
        /// </summary>
        /// <param name="sender">Objeto que disparó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void BtnAccederPedidos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_publicidadVista)
                {
                    MessageBox.Show("Debe ver la publicidad antes de acceder al sistema de pedidos.", 
                        "Acceso Denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _logger.LogInformation("Usuario accediendo al sistema de pedidos");

                // Crear y mostrar ventana de pedidos
                var pedidosWindow = new PedidosWindow(_serviceProvider, _configuration);
                
                // Ocultar ventana principal
                Hide();
                
                // Mostrar ventana de pedidos
                pedidosWindow.Show();
                
                // Suscribirse al evento de cierre para mostrar nuevamente la ventana principal
                pedidosWindow.Closed += (s, args) =>
                {
                    _logger.LogInformation("Ventana de pedidos cerrada, mostrando ventana principal");
                    Show();
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al abrir ventana de pedidos");
                MessageBox.Show($"Error al acceder al sistema de pedidos: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Show(); // Asegurar que la ventana principal sea visible en caso de error
            }
        }

        /// <summary>
        /// Sobrescribir el método de cierre para confirmar la salida
        /// </summary>
        /// <param name="e">Argumentos del evento de cierre</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("¿Está seguro que desea salir de la aplicación?", 
                    "Confirmar Salida", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }

                _logger.LogInformation("Cerrando aplicación por solicitud del usuario");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el cierre de la aplicación");
            }

            base.OnClosing(e);
        }
    }
}