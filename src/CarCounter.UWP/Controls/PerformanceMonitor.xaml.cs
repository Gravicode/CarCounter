using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed class PerformanceMetric
    {
        public string Title { get; set; }
        public string Duration { get; set; }
    }
    public sealed partial class PerformanceMonitor : UserControl
    {
        private ObservableCollection<PerformanceMetric> Items { get; } = new ObservableCollection<PerformanceMetric>();

        public PerformanceMonitor()
        {
            this.InitializeComponent();

            this.Visibility = Visibility.Collapsed;
        }

        public void Log(string title, float duration)
        {
            var duationString = String.Format("{0:00.0000}", duration);
            Items.Add(new PerformanceMetric { Title = title, Duration = duationString });
            this.Visibility = Visibility.Visible;
        }

        public void ClearLog()
        {
            Items.Clear();
        }
    }
}
