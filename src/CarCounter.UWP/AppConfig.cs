using CarCounter.UWP.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace CarCounter.UWP
{
    public class AppConfig
    {
        public AppConfig()
        {
           
        }
        public static void Load()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (localSettings != null)
            {
                var testVal = localSettings.Values["Cctv1"] as string;
                if (string.IsNullOrEmpty(testVal)) return;
                // load a setting that is local to the device
                AppConstants.Cctv1 = localSettings.Values["Cctv1"] as string;
                AppConstants.CctvHttp = localSettings.Values["CctvHttp"] as string;
                AppConstants.Username = localSettings.Values["Username"] as string;
                AppConstants.Password = localSettings.Values["Password"] as string;
                AppConstants.SelectionArea = localSettings.Values["SelectionArea"] as string;
                bool.TryParse( localSettings.Values["AutoStart"] as string, out var autostart);
                AppConstants.AutoStart = autostart;
            }
        }
        public static void Save()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            // Save a setting locally on the device
            localSettings.Values["Cctv1"] = AppConstants.Cctv1;
            localSettings.Values["CctvHttp"] = AppConstants.CctvHttp;
            localSettings.Values["Username"] = AppConstants.Username;
            localSettings.Values["Password"] = AppConstants.Password;
            localSettings.Values["SelectionArea"] = AppConstants.SelectionArea;
            localSettings.Values["AutoStart"] = AppConstants.AutoStart.ToString();

        }
    }
}
