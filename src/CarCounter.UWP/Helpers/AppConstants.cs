using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCounter.UWP.Helpers
{
    public class AppConstants
    {
        public static string Cctv1 { set; get; } = "rtsp://admin:123qweasd!@192.168.68.113:554/Streaming/Channels/101"; //"rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4";//"rtsp://demo:demo@ipvmdemo.dyndns.org:5541/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=unicast";
        public static string CctvHttp { set; get; } = "http://192.168.68.6/ISAPI/Streaming/channels/101/picture";
        public static string Username { set; get; } = "admin";
        public static string Password { set; get; } = "123qweasd!";
        public static string SelectionArea { get; set; }
        public static string Gateway { get; set; } = "Gateway-001";
        public static string Lokasi { get; set; } = "Botani Square Bogor";
        public static string GrpcUrl { get; set; } = "https://localhost:7091/";
    }
}
