using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYP_Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Location = FYP_Mobile.Models.Location;

namespace FYP_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedLocation : ContentPage
    {
        private ObservableCollection<Note> _notes = new ObservableCollection<Note>();
        private Location _storedLocation { get; set; }

        public ObservableCollection<Note> Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        public SelectedLocation(Location selectedLocation)
        {
            InitializeComponent();
            _storedLocation = selectedLocation;
            locationLabel.Text = selectedLocation.Label;

            NotesList.ItemsSource = _notes;
            selectedLocation.Notes.ForEach(_notes.Add);

        }

        public void OnMapViewClicked(object sender, EventArgs e)
        {
            var location =
                new Xamarin.Essentials.Location((double) _storedLocation.Latitude, (double) _storedLocation.Longitude);
            var options = new MapLaunchOptions {Name = _storedLocation.Label};
            Map.OpenAsync(location, options);
        }

        public void OnAddNoteClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new AddNote(_storedLocation.Id));
        }
    }
}