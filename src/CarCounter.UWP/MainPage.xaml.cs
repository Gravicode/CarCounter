using CarCounter.Models;
using CarCounter.Tools;
using CarCounter.UWP.Helpers;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AI.Skills.Vision.ObjectDetector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Rectangle = Windows.UI.Xaml.Shapes.Rectangle;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CarCounter.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            frameMainContent.Navigate(typeof(CctvPage), this);
        }            

        private void btnShowPane_Click(object sender, RoutedEventArgs e)
        {
            mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
            if (!mainSplitView.IsPaneOpen)
            {
                btnShowPaneIcon.Glyph = "\ue76c";
                btnShowPane.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                btnShowPaneIcon.Glyph = "\ue76b";
                btnShowPane.HorizontalAlignment = HorizontalAlignment.Right;
            }
        }

        private void btnCctv_Click(object sender, RoutedEventArgs e)
        {
            frameMainContent.Navigate(typeof(CctvPage), this);
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            frameMainContent.Navigate(typeof(ConfigPage), this);
        }   
    }
}
