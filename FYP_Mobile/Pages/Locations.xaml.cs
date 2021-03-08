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
using FYP_Mobile.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FYP_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Locations : ContentPage
    {
        private ObservableCollection<Location> _locations = new ObservableCollection<Location>();
        public ObservableCollection<Location> LocationsList
        {
            get { return _locations; }
        }

        public Locations()
        {
            InitializeComponent();
            LocationsView.ItemsSource = _locations;
            var getLocations = GetLocations();
            getLocations.ForEach(_locations.Add);
        }

        public List<Location> GetLocations()
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(UrlHelper.LocationUrl);
            webRequest.ServerCertificateValidationCallback = delegate { return true; };
            webRequest.Method = "GET";
            webRequest.AllowAutoRedirect = false;
            webRequest.ContentLength = 0;

            var response = (HttpWebResponse)webRequest.GetResponse();

            List<Location> locationList = null;

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                locationList = JsonConvert.DeserializeObject<List<Location>>(reader.ReadToEnd());
            }


            return locationList;
        }

        private void LocationsListItemTapped(object sender, ItemTappedEventArgs e)
        {
            var index = LocationsList.IndexOf(e.Item as Location);
            var selectedItem = (Location) e.Item;
            App.Current.MainPage = new NavigationPage(new SelectedLocation(selectedItem));
        }
    }
}