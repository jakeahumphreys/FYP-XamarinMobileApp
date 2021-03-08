using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FYP_Mobile.Common;
using FYP_Mobile.DTO;
using FYP_Mobile.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FYP_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersPage : ContentPage
    {
        private ObservableCollection<MobileUserDto> _users = new ObservableCollection<MobileUserDto>();

        public ObservableCollection<MobileUserDto> UserList
        {
            get { return _users; }
        }

        public UsersPage()
        {
            InitializeComponent();
            UsersView.ItemsSource = _users;
            var getUsers = GetUsers();
            getUsers.ForEach(_users.Add);

        }

        public List<MobileUserDto> GetUsers()
        {
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(UrlHelper.LoginUrl);
                webRequest.ServerCertificateValidationCallback = delegate { return true; };
                webRequest.Method = "GET";
                webRequest.AllowAutoRedirect = false;
                webRequest.ContentLength = 0;

                var response = (HttpWebResponse)webRequest.GetResponse();

                List<MobileUserDto> usersList = null;

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    usersList = JsonConvert.DeserializeObject<List<MobileUserDto>>(reader.ReadToEnd());
                }


                return usersList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
          
        }

        private void UserListItemTapped(object sender, ItemTappedEventArgs e)
        {
            var index = UserList.IndexOf(e.Item as MobileUserDto);
            var selectedItem = (MobileUserDto) e.Item;
            App.Current.MainPage = new NavigationPage(new SendRoutineMessage(selectedItem));
        }
    }
}