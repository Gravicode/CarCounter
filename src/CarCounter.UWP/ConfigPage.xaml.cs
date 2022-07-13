using CarCounter.UWP.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CarCounter.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigPage : Page
    {
        public ConfigPage()
        {
            this.InitializeComponent();
            LoadConfig();
        }

        void LoadConfig()
        {
            LoadConfiguration();
        }

        void LoadConfiguration()
        {
            AppConfig.Load();

            InpGateway.Text = AppConstants.Gateway;
            InpLokasi.Text = AppConstants.Lokasi;
            InpCctv1.Text = AppConstants.Cctv1;
            InpGrpc.Text = AppConstants.GrpcUrl;
        }

        void SaveConfiguration()
        {
            AppConstants.Gateway = InpGateway.Text;
            AppConstants.Lokasi = InpLokasi.Text;
            AppConstants.Cctv1 = InpCctv1.Text;
            AppConstants.GrpcUrl = InpGrpc.Text;

            AppConfig.Save();
        }

        private async void btnSaveConfig_Click(object sender, RoutedEventArgs e)
        {
            SaveConfiguration();
            //await CoreApplication.RequestRestartAsync("Restarting apps");
        }
    }
}
