using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCounter.UWP.Helpers
{
    public class AppConstants
    {
        public static string Cctv1 { set; get; } = "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4";//"rtsp://demo:demo@ipvmdemo.dyndns.org:5541/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=unicast";
        public static string CctvHttp { set; get; } = "http://192.168.68.6/ISAPI/Streaming/channels/101/picture";
        public static string Username { set; get; } = "admin";
        public static string Password { set; get; } = "123qweasd!";
    }
}
