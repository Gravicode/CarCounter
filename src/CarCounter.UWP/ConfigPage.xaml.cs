using CarCounter.UWP.Data;
using CarCounter.UWP.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CarCounter.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigPage : Page
    {
        readonly ILogger<ConfigPage> _logger;
        public ConfigPage()
        {
            _logger = DI.Pool.GetService<ILoggerFactory>()
                .CreateLogger<ConfigPage>();
            _logger.LogInformation("Start Config page");
            this.InitializeComponent();
            LoadConfig();
        }

        void LoadConfig()
        {
            LoadConfiguration();
        }

        void LoadConfiguration()
        {
            _logger.LogInformation("Commencing load configuration");
            AppConfig.Load();
            InpGateway.Text = AppConstants.Gateway;
            InpLokasi.Text = AppConstants.Lokasi;
            InpCctv1.Text = AppConstants.Cctv1;
            InpGrpc.Text = AppConstants.GrpcUrl;
            _logger.LogInformation("Load configuration succeed");
        }

        void SaveConfiguration()
        {
            _logger.LogInformation("Commencing save configuration");
            AppConstants.Gateway = InpGateway.Text;
            AppConstants.Lokasi = InpLokasi.Text;
            AppConstants.Cctv1 = InpCctv1.Text;
            AppConstants.GrpcUrl = InpGrpc.Text;
            AppConfig.Save();
            _logger.LogInformation("Save configuration succeed");
        }

        private async void btnSaveConfig_Click(object sender, RoutedEventArgs e)
        {
            SaveConfiguration();
            //await CoreApplication.RequestRestartAsync("Restarting apps");
        }
    }
}
