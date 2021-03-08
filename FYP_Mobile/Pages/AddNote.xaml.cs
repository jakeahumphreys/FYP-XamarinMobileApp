using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
using FYP_Mobile.Common;
using FYP_Mobile.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FYP_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNote : ContentPage
    {
        private int _locationId { get; set; }
        public AddNote(int locationId)
        {
            InitializeComponent();
            _locationId = locationId;
        }

        public void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            var url = UrlHelper.NoteUrl + $"/?storedLocationId={_locationId}";
            var webRequest = (HttpWebRequest) WebRequest.Create(url);
            webRequest.ServerCertificateValidationCallback = delegate { return true; };
            webRequest.Method = "POST";
            webRequest.AllowAutoRedirect = false;
            webRequest.ContentType = "application/json";

            var noteRequest = new Note
            {
                Content = NoteContent.Text
            };

            var jsonString = JsonConvert.SerializeObject(noteRequest);
            webRequest.ContentLength = jsonString.Length;

            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonString);
            }


            var response = (HttpWebResponse)webRequest.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                App.Current.MainPage = new NavigationPage(new ApplicationHome());
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                DisplayAlert("Error", response.StatusDescription, "Ok");
            }
        }
    }

  
}