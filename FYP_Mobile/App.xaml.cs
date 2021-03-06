using System;
using System.IO;
using FYP_Mobile.Common;
using FYP_Mobile.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FYP_Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {

            if (!File.Exists(FileHelper.UserFile))
            {
                MainPage = new LoginPage();
            }
            else
            {
                MainPage = new NavigationPage(new ApplicationHome());
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
