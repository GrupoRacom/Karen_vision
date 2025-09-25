using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KarenVision.Views
{
    /// <summary>
    /// Ventana de publicidad de la aplicación Karen Vision
    /// Muestra contenido publicitario con temporizador y opciones de interacción
    /// </summary>
    public partial class PublicidadWindow : Window
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly DispatcherTimer _timer;
        private readonly DispatcherTimer _animationTimer;
        private int _tiempoRestante;
        private int _duracionTotal;
        private bool _publicidadCompletada = false;

        /// <summary>
        /// Evento que se dispara cuando la publicidad ha sido completada
        /// </summary>
        public event EventHandler? PublicidadCompletada;

        /// <summary>
        /// Constructor de la ventana de publicidad
        /// </summary>
        /// <param name="configuration">Configuración de la aplicación</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public PublicidadWindow(IConfiguration configuration, ILogger logger)
        {
            InitializeComponent();
            
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Obtener duración de la publicidad desde configuración
            _duracionTotal = _configuration.GetValue<int>("AppSettings:AdvertisementDuration", 10);
            _tiempoRestante = _duracionTotal;

            // Configurar temporizador principal
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;

            // Configurar temporizador para animaciones
            _animationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(2000)
            };
            _animationTimer.Tick += AnimationTimer_Tick;

            _logger.LogInformation("Ventana de publicidad inicializada con duración: {Duracion}s", _duracionTotal);
            
            // Inicializar interfaz
            InicializarInterfaz();
            
            // Iniciar temporizadores
            _timer.Start();
            _animationTimer.Start();
            
            // Iniciar animación del texto
            IniciarAnimacionTexto();
        }

        /// <summary>
        /// Inicializa los elementos de la interfaz de usuario
        /// </summary>
        private void InicializarInterfaz()
        {
            try
            {
                // Actualizar textos iniciales
                TxtTiempoRestante.Text = $"Tiempo restante: {_tiempoRestante} segundos";
                TxtContador.Text = _tiempoRestante.ToString();
                
                // Configurar progreso inicial
                ProgressBarTiempo.Value = 0;
                
                // El botón de saltar se habilita después de 5 segundos
                BtnSaltarPublicidad.IsEnabled = false;
                
                _logger.LogDebug("Interfaz de publicidad inicializada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al inicializar interfaz de publicidad");
            }
        }

        /// <summary>
        /// Inicia la animación del texto promocional
        /// </summary>
        private void IniciarAnimacionTexto()
        {
            try
            {
                // Animación simplificada - se puede expandir en el futuro
                _logger.LogDebug("Animación de texto iniciada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar animación de texto");
            }
        }

        /// <summary>
        /// Manejador del temporizador principal
        /// Actualiza el contador y controla la duración de la publicidad
        /// </summary>
        /// <param name="sender">Objeto que disparó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void Timer_Tick(object? sender, EventArgs e)
        {
            try
            {
                _tiempoRestante--;
                
                // Actualizar interfaz
                TxtTiempoRestante.Text = $"Tiempo restante: {_tiempoRestante} segundos";
                TxtContador.Text = _tiempoRestante.ToString();
                
                // Actualizar barra de progreso
                double progreso = ((double)(_duracionTotal - _tiempoRestante) / _duracionTotal) * 100;
                ProgressBarTiempo.Value = progreso;
                
                // Habilitar botón de saltar después de 5 segundos
                if (_tiempoRestante <= _duracionTotal - 5)
                {
                    BtnSaltarPublicidad.IsEnabled = true;
                    BtnSaltarPublicidad.Content = "⏭ Saltar";
                }
                
                // Completar publicidad cuando el tiempo llegue a 0
                if (_tiempoRestante <= 0)
                {
                    _timer.Stop();
                    CompletarPublicidad();
                }
                
                _logger.LogDebug("Tiempo restante de publicidad: {TiempoRestante}s", _tiempoRestante);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en temporizador de publicidad");
            }
        }

        /// <summary>
        /// Manejador del temporizador de animaciones
        /// Cambia el contenido promocional periódicamente
        /// </summary>
        /// <param name="sender">Objeto que disparó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                // Lista de mensajes promocionales
                string[] mensajes = {
                    "🎉 ¡No te pierdas nuestras increíbles ofertas! 🎉",
                    "⭐ Productos de la más alta calidad ⭐",
                    "🚚 Envío gratis en pedidos mayores a $50 🚚",
                    "💰 Descuentos especiales para clientes frecuentes 💰",
                    "🎁 ¡Sorpresas en cada compra! 🎁"
                };
                
                // Seleccionar mensaje aleatorio
                Random random = new Random();
                string nuevoMensaje = mensajes[random.Next(mensajes.Length)];
                TxtMensajeAnimado.Text = nuevoMensaje;
                
                _logger.LogDebug("Mensaje promocional actualizado: {Mensaje}", nuevoMensaje);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en temporizador de animaciones");
            }
        }

        /// <summary>
        /// Manejador del evento de clic en el botón "Saltar Publicidad"
        /// </summary>
        /// <param name="sender">Objeto que disparó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void BtnSaltarPublicidad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _logger.LogInformation("Usuario saltó la publicidad");
                CompletarPublicidad();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al saltar publicidad");
                MessageBox.Show($"Error al saltar publicidad: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Manejador del evento de clic en el botón "Me Interesa"
        /// Muestra información adicional y completa la publicidad
        /// </summary>
        /// <param name="sender">Objeto que disparó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void BtnInteractuar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _logger.LogInformation("Usuario interactuó con la publicidad");
                
                // Mostrar mensaje de agradecimiento
                var result = MessageBox.Show(
                    "¡Gracias por tu interés en nuestros productos!\n\n" +
                    "Te invitamos a explorar nuestro catálogo completo en el sistema de pedidos.\n" +
                    "Encontrarás productos de calidad a precios increíbles.\n\n" +
                    "¿Deseas continuar al sistema de pedidos?",
                    "¡Gracias por tu Interés!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);

                // Completar publicidad independientemente de la respuesta
                CompletarPublicidad();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar interacción con publicidad");
                MessageBox.Show($"Error al procesar interacción: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Completa la visualización de la publicidad y notifica al sistema
        /// </summary>
        private void CompletarPublicidad()
        {
            try
            {
                if (_publicidadCompletada)
                    return;

                _publicidadCompletada = true;
                
                // Detener temporizadores
                _timer.Stop();
                _animationTimer.Stop();
                
                _logger.LogInformation("Publicidad completada exitosamente");
                
                // Notificar a la ventana principal
                PublicidadCompletada?.Invoke(this, EventArgs.Empty);
                
                // Cerrar ventana con resultado positivo
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al completar publicidad");
                MessageBox.Show($"Error al completar publicidad: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Sobrescribir el comportamiento de cierre para evitar cierre accidental
        /// </summary>
        /// <param name="e">Argumentos del evento de cierre</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                // Si la publicidad no ha sido completada y el usuario intenta cerrar
                if (!_publicidadCompletada && DialogResult != true)
                {
                    var result = MessageBox.Show(
                        "¿Está seguro que desea cerrar la publicidad?\n" +
                        "Debe ver la publicidad completa para acceder al sistema de pedidos.",
                        "Confirmar Cierre",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result != MessageBoxResult.Yes)
                    {
                        e.Cancel = true;
                        return;
                    }
                    
                    // Si confirma el cierre, detener temporizadores
                    _timer.Stop();
                    _animationTimer.Stop();
                }
                
                _logger.LogDebug("Ventana de publicidad cerrándose");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el cierre de ventana de publicidad");
            }

            base.OnClosing(e);
        }
    }
}