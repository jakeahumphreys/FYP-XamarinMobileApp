using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYP_Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FYP_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            var loginViewModel = new LoginViewModel();
            this.BindingContext = loginViewModel;
            loginViewModel.DisplayInvalidLoginPrompt += () => DisplayAlert("Login Error", "Invalid Credentials", "Ok");
            InitializeComponent();

            Username.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };

            Password.Completed += (object sender, EventArgs e) =>
            {
                loginViewModel.SubmitCommand.Execute(null);
            };
        }
    }
}