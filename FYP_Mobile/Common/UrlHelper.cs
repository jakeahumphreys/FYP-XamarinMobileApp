using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace FYP_Mobile.Common
{
    public static class UrlHelper
    {
        public static string ApiUrl = "https://192.168.0.24:45455/api/";
        public static string LoginUrl = ApiUrl + "Account/";
        public static string LocationUrl = ApiUrl + "StoredLocation";
        public static string NoteUrl = ApiUrl + "Note";
    }
}
