using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYP_Mobile.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FYP_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApplicationHome : ContentPage
    {
        public ApplicationHome()
        {
            InitializeComponent();
        }

        public void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            if (File.Exists(FileHelper.UserFile))
            {
                File.Delete(FileHelper.UserFile);
                App.Current.MainPage = new LoginPage();
            }
        }
    }
}