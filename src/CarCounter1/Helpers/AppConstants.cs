using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CarCounter1.Helpers
{
    public class AppConstants
    {
        public static string GrpcUrl = "";
        public static string Gateway = "";
        public static string Lokasi = "";
        //key-pair
        public const string Authentication = "auth";

        public const int FACE_WIDTH = 180;
        public const int FACE_HEIGHT = 135;
        public const string FACE_SUBSCRIPTION_KEY = "a068e60df8254cc5a187e3e8c644f316";
        public const string FACE_ENDPOINT = "https://southeastasia.api.cognitive.microsoft.com/";
        public static string ReportPJKBM = "";
        public static string ReportKPI = "";

        public static string KUReportUrl = "";
        public static string KUInfaqBulananUrl = "";
        public static string SQLConn = "";
        public static string JamMasjidUrl = "https://jam-masjid.azurewebsites.net/";
        public static string BlobConn { get; set; }
        public const string GemLic = "EDWG-SKFA-D7J1-LDQ5";
       
        public static string RedisCon { set; get; }
      
    }
}
