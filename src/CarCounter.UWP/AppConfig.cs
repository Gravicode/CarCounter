using CarCounter.Models;
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
                InitSetting();
                // load a setting that is local to the device
                AppConstants.Gateway = localSettings.Values[LocalSettingName.Gateway] as string;
                AppConstants.Lokasi = localSettings.Values[LocalSettingName.Lokasi] as string;
                AppConstants.GrpcUrl = localSettings.Values[LocalSettingName.GrpcUrl] as string;
                AppConstants.Cctv1 = localSettings.Values[LocalSettingName.Cctv1] as string;
                AppConstants.CctvHttp = localSettings.Values[LocalSettingName.Cctv2] as string;
                AppConstants.Username = localSettings.Values[LocalSettingName.Username] as string;
                AppConstants.Password = localSettings.Values[LocalSettingName.Password] as string;
                AppConstants.SelectionArea = localSettings.Values[LocalSettingName.SelectionArea] as string;
                bool.TryParse(localSettings.Values[LocalSettingName.AutoStart] as string, out var autostart);
                AppConstants.AutoStart = autostart;
            }
        }

        public static void Save()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            // Save a setting locally on the device
            localSettings.Values[LocalSettingName.Gateway] = AppConstants.Gateway;
            localSettings.Values[LocalSettingName.Lokasi] = AppConstants.Lokasi;
            localSettings.Values[LocalSettingName.Cctv1] = AppConstants.Cctv1;
            localSettings.Values[LocalSettingName.Cctv2] = AppConstants.CctvHttp;
            localSettings.Values[LocalSettingName.Username] = AppConstants.Username;
            localSettings.Values[LocalSettingName.Password] = AppConstants.Password;
            localSettings.Values[LocalSettingName.SelectionArea] = AppConstants.SelectionArea;
            localSettings.Values[LocalSettingName.AutoStart] = AppConstants.AutoStart.ToString();
            localSettings.Values[LocalSettingName.GrpcUrl] = AppConstants.GrpcUrl;
        }

        public static void InitSetting()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey("IsFirstLoad"))
            {
                localSettings.Values["IsFirstLoad"] = "true";

                localSettings.Values[LocalSettingName.Gateway] = "Gateway-001";
                localSettings.Values[LocalSettingName.Lokasi] = "Botani Square Bogor";
                localSettings.Values[LocalSettingName.Cctv1] = "rtsp://admin:123qweasd!@192.168.68.113:554/Streaming/Channels/101";
                localSettings.Values[LocalSettingName.Cctv2] = "http://192.168.68.6/ISAPI/Streaming/channels/101/picture";
                localSettings.Values[LocalSettingName.Username] = "admin";
                localSettings.Values[LocalSettingName.Password] = "123qweasd!";
                localSettings.Values[LocalSettingName.SelectionArea] = "";
                localSettings.Values[LocalSettingName.AutoStart] = "false";
                localSettings.Values[LocalSettingName.GrpcUrl] = "https://carcounterapi.azurewebsites.net/";
            }
        }
    }

    public static class LocalSettingName
    {
        public static readonly string Gateway = "Gateway";
        public static readonly string Lokasi = "Lokasi";
        public static readonly string AutoStart = "AutoStart";
        public static readonly string GrpcUrl = "GrpcUrl";
        public static readonly string SelectionArea = "SelectionArea";
        public static readonly string Cctv1 = "Cctv1";
        public static readonly string Cctv2 = "Cctv2";
        public static readonly string Cctv3 = "Cctv3";
        public static readonly string Cctv4 = "Cctv4";
        public static readonly string Username = "Username";
        public static readonly string Password = "Password";

    }
}
