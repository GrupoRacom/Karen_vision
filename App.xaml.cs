using System;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using KarenVision.Data;
using KarenVision.Services;
using KarenVision.Views;

namespace KarenVision
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// Configuración principal de la aplicación WPF con inyección de dependencias
    /// </summary>
    public partial class App : Application
    {
        private IHost? _host;

        /// <summary>
        /// Configuración inicial de la aplicación
        /// </summary>
        /// <param name="e">Argumentos de inicio</param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                // Configurar el host con inyección de dependencias
                _host = Host.CreateDefaultBuilder()
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    })
                    .ConfigureServices((context, services) =>
                    {
                        // Configurar servicios
                        ConfigureServices(services, context.Configuration);
                    })
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole();
                        logging.AddDebug();
                        logging.SetMinimumLevel(LogLevel.Information);
                    })
                    .UseConsoleLifetime()
                    .Build();

                await _host.StartAsync();

                // Mostrar ventana principal
                var mainWindow = _host.Services.GetRequiredService<MainWindow>();
                mainWindow.Show();

                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar la aplicación: {ex.Message}", 
                    "Error de Inicio", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(1);
            }
        }

        /// <summary>
        /// Configuración de servicios de inyección de dependencias
        /// </summary>
        /// <param name="services">Colección de servicios</param>
        /// <param name="configuration">Configuración de la aplicación</param>
        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Registrar configuración
            services.AddSingleton(configuration);

            // Registrar contexto de base de datos
            services.AddScoped<KarenVisionContext>();

            // Registrar servicios de negocio
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IPedidoService, PedidoService>();

            // Registrar ventanas
            services.AddTransient<MainWindow>();
            services.AddTransient<PublicidadWindow>();
            services.AddTransient<PedidosWindow>();
        }

        /// <summary>
        /// Limpieza al cerrar la aplicación
        /// </summary>
        /// <param name="e">Argumentos de cierre</param>
        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }

            base.OnExit(e);
        }
    }
}