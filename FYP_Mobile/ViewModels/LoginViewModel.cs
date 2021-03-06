using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Input;
using FYP_Mobile.Common;
using FYP_Mobile.DTO;
using FYP_Mobile.Pages;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;

namespace FYP_Mobile.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public Action DisplayInvalidLoginPrompt;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private string username;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        public ICommand SubmitCommand { protected set; get; }

        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
        }

        public void OnSubmit()
        {
            try
            {
                var url = UrlHelper.LoginUrl + $"Login?username={Username}&loginKey={Password}";
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ServerCertificateValidationCallback = delegate { return true; };
                webRequest.Method = "POST";
                webRequest.AllowAutoRedirect = false;
                webRequest.ContentLength = 0;

                var response = (HttpWebResponse)webRequest.GetResponse();

                MobileUserDto user = null;

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    user = JsonConvert.DeserializeObject<MobileUserDto>(reader.ReadToEnd());
                }

                SaveFile(user);
                App.Current.MainPage = new ApplicationHome();
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError && e.Response != null)
                {
                    var response = (HttpWebResponse) e.Response;
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        App.Current.MainPage.DisplayAlert("Invalid Credentials",
                            "The details you entered were not correct, please use the details provided by your line manager.", "Ok");
                    }
                }
            }
          
        }

        public void SaveFile(MobileUserDto user)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "user.json");
            File.WriteAllText(fileName,JsonConvert.SerializeObject(user));
        }
    }
}
