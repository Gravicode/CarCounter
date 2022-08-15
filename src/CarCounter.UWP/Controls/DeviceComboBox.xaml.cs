using CarCounter.UWP.Helpers;
using Microsoft.AI.MachineLearning;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CarCounter.UWP.Controls
{
    public sealed partial class DeviceComboBox : UserControl
    {
        bool _firstLoad = true;
        public int SelectedIndex = 0;
        public DeviceComboBox()
        {
            this.InitializeComponent();
            AppConfig.Load();

            DeviceBox.SelectedIndex = SelectedIndex = AppConstants.ProcessingTarget;
            _firstLoad = false;
        }

        private void changeSelectedIndex(object sender, RoutedEventArgs e)
        {
            SelectedIndex = DeviceBox.SelectedIndex;
        }

        public LearningModelDeviceKind GetDeviceKind()
        {
            if (SelectedIndex == 0)
                return LearningModelDeviceKind.Cpu;
            else
                return LearningModelDeviceKind.DirectXHighPerformance;
        }

        private void changeSelectedIndex(object sender, SelectionChangedEventArgs e)
        {
            if (!_firstLoad)
            {
                var combobox = sender as ComboBox;
                SelectedIndex = combobox.SelectedIndex;

                DisplayChangeProcessingTagretDialog();
            }
        }

        private async void DisplayChangeProcessingTagretDialog()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Need to Restart!!",
                Content = "Are you sure want to restart this app, due to change processing target?",
                PrimaryButtonText = "Yes, Restart",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                AppConstants.ProcessingTarget = SelectedIndex;
                AppConfig.Save();
                await CoreApplication.RequestRestartAsync("Restarting apps");
            }
            else
            {
            }
        }
    }
}
