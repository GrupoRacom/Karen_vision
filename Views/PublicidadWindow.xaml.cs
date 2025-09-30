using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Shapes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KarenVision.Views
{
    /// <summary>
    /// Ventana de publicidad de la aplicación Karen Vision
    /// Muestra múltiples imágenes publicitarias con temporizador y rotación automática
    /// </summary>
    public partial class PublicidadWindow : Window
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly DispatcherTimer _timer;
        private readonly DispatcherTimer _imageTimer;
        private readonly DispatcherTimer _animationTimer;
        private int _tiempoRestante;
        private int _duracionTotal;
        private int _intervaloImagenes;
        private bool _publicidadCompletada = false;
        
        // Sistema de imágenes
        private List<string> _imagenes;
        private int _imagenActual = 0;
        private List<Ellipse> _indicadores;

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

            // Obtener configuración
            _duracionTotal = _configuration.GetValue<int>("AppSettings:AdvertisementDuration", 10);
            _intervaloImagenes = _configuration.GetValue<int>("AppSettings:ImageChangeInterval", 3);
            _tiempoRestante = _duracionTotal;

            // Configurar temporizador principal
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;

            // Configurar temporizador de imágenes
            _imageTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(_intervaloImagenes)
            };
            _imageTimer.Tick += ImageTimer_Tick;

            // Configurar temporizador de animación
            _animationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _animationTimer.Tick += AnimationTimer_Tick;

            // Inicializar sistema de imágenes
            InicializarImagenes();
            
            _logger.LogInformation("Ventana de publicidad inicializada con duración: {Duracion}s", _duracionTotal);
        }

        /// <summary>
        /// Inicializa el sistema de imágenes publicitarias
        /// </summary>
        private void InicializarImagenes()
        {
            try
            {
                _imagenes = new List<string>();
                _indicadores = new List<Ellipse>();

                // Buscar imágenes en la carpeta Assets/Images
                string imageFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Images", "Publicidad");
                
                if (Directory.Exists(imageFolder))
                {
                    string[] extensions = { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.gif" };
                    foreach (string extension in extensions)
                    {
                        string[] files = Directory.GetFiles(imageFolder, extension);
                        _imagenes.AddRange(files);
                    }
                }

                // Si no hay imágenes, usar una imagen por defecto
                if (_imagenes.Count == 0)
                {
                    _logger.LogWarning("No se encontraron imágenes en {ImageFolder}", imageFolder);
                }

                _logger.LogDebug("Sistema de imágenes inicializado con {Count} imágenes", _imagenes.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al inicializar sistema de imágenes");
                _imagenes = new List<string>();
                _indicadores = new List<Ellipse>();
            }
        }

        /// <summary>
        /// Carga una imagen específica en el visor
        /// </summary>
        private void CargarImagen(int indice)
        {
            try
            {
                if (indice >= 0 && indice < _imagenes.Count)
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(_imagenes[indice], UriKind.RelativeOrAbsolute);
                    bitmap.EndInit();
                    
                    // Asignar imagen al control (asumiendo que hay un Image llamado ImgPublicidad)
                    // ImgPublicidad.Source = bitmap;
                    
                    _logger.LogDebug("Imagen cargada: {ImagePath}", _imagenes[indice]);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar imagen {Index}", indice);
            }
        }

        /// <summary>
        /// Manejador de eventos que se ejecuta cuando la ventana está cargada
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                InicializarInterfaz();
                IniciarAnimacionTexto();
                
                // Iniciar temporizadores
                _timer.Start();
                _imageTimer.Start();
                _animationTimer.Start();
                
                _logger.LogInformation("Publicidad iniciada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar ventana de publicidad");
                MessageBox.Show($"Error al cargar publicidad: {ex.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Inicializa los elementos de la interfaz de usuario
        /// </summary>
        private void InicializarInterfaz()
        {
            try
            {
                // Configurar barra de progreso
                if (ProgressBarTiempo != null)
                {
                    ProgressBarTiempo.Maximum = _duracionTotal;
                    ProgressBarTiempo.Value = 0;
                }

                // Deshabilitar botón de saltar al inicio
                if (BtnSaltarPublicidad != null)
                    BtnSaltarPublicidad.IsEnabled = false;
                
                _logger.LogDebug("Interfaz de publicidad inicializada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al inicializar interfaz de publicidad");
            }
        }

        /// <summary>
        /// Inicia las animaciones de texto
        /// </summary>
        private void IniciarAnimacionTexto()
        {
            try
            {
                // Implementar animación de texto si es necesario
                _logger.LogDebug("Animación de texto iniciada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar animación de texto");
            }
        }

        /// <summary>
        /// Manejador del temporizador principal que controla el tiempo de la publicidad
        /// </summary>
        private void Timer_Tick(object? sender, EventArgs e)
        {
            try
            {
                _tiempoRestante--;

                // Actualizar interfaz
                if (TxtTiempoRestante != null)
                    TxtTiempoRestante.Text = $"Tiempo restante: {_tiempoRestante} segundos";
                
                if (TxtContador != null)
                    TxtContador.Text = _tiempoRestante.ToString();

                if (ProgressBarTiempo != null)
                    ProgressBarTiempo.Value = _duracionTotal - _tiempoRestante;

                // Habilitar botón de saltar cuando queden pocos segundos
                if (_tiempoRestante <= 3 && BtnSaltarPublicidad != null)
                    BtnSaltarPublicidad.IsEnabled = true;

                // Completar publicidad cuando se acabe el tiempo
                if (_tiempoRestante <= 0)
                {
                    CompletarPublicidad();
                }

                _logger.LogDebug("Tiempo restante: {TiempoRestante}s", _tiempoRestante);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en temporizador principal");
            }
        }

        /// <summary>
        /// Manejador del temporizador de cambio de imágenes
        /// </summary>
        private void ImageTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (_imagenes.Count > 1)
                {
                    _imagenActual = (_imagenActual + 1) % _imagenes.Count;
                    CargarImagen(_imagenActual);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar imagen");
            }
        }

        /// <summary>
        /// Manejador del temporizador de animaciones
        /// </summary>
        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                // Implementar animaciones adicionales si es necesario
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en animaciones");
            }
        }

        /// <summary>
        /// Manejador del evento de clic en el botón "Saltar Publicidad"
        /// </summary>
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
        /// </summary>
        private void BtnInteractuar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _logger.LogInformation("Usuario interactuó con la publicidad");
                
                var result = MessageBox.Show(
                    "¡Gracias por tu interés en nuestros productos!\n\n" +
                    "Te invitamos a explorar nuestro catálogo completo en el sistema de pedidos.\n" +
                    "Encontrarás productos de calidad a precios increíbles.\n\n" +
                    "¿Deseas continuar al sistema de pedidos?",
                    "¡Gracias por tu Interés!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);

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
        /// Completa la visualización de la publicidad
        /// </summary>
        private void CompletarPublicidad()
        {
            try
            {
                if (_publicidadCompletada)
                    return;

                _publicidadCompletada = true;
                
                _timer.Stop();
                _imageTimer.Stop();
                _animationTimer.Stop();
                
                _logger.LogInformation("Publicidad completada exitosamente");
                
                PublicidadCompletada?.Invoke(this, EventArgs.Empty);
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
        /// Sobrescribir el comportamiento de cierre
        /// </summary>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
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
                    
                    _timer.Stop();
                    _imageTimer.Stop();
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