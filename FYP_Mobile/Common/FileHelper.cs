using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FYP_Mobile.Common
{
    public static class FileHelper
    {
        public static string UserFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "user.json");
    }
}
