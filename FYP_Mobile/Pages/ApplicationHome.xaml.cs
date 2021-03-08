using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FYP_Mobile.Common;
using FYP_Mobile.DTO;
using FYP_Mobile.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FYP_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApplicationHome : ContentPage
    {
        private CancellationTokenSource cts;
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

        public void OnLocationButtonClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new Locations();
        }

        async void ReportGpsLocation(object sender, EventArgs e)
        {
            MobileUserDto user = JsonConvert.DeserializeObject<MobileUserDto>(File.ReadAllText(FileHelper.UserFile));
            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            cts = new CancellationTokenSource();
            var location = await Geolocation.GetLocationAsync(request, cts.Token);

            if (location != null)
            {
                var report = new GpsReport
                {
                    Time = DateTime.Now,
                    Latitude = (decimal) location.Latitude,
                    Longitude = (decimal) location.Longitude,
                    UserId = user.Id
                };

                var url = UrlHelper.GpsReportUrl;
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ServerCertificateValidationCallback = delegate { return true; };
                webRequest.Method = "POST";
                webRequest.AllowAutoRedirect = false;
                webRequest.ContentType = "application/json";

                var jsonString = JsonConvert.SerializeObject(report);
                webRequest.ContentLength = jsonString.Length;

                using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonString);
                }


                var response = (HttpWebResponse)webRequest.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    await DisplayAlert("Location Reported", "Your location has been successfully reported to your manager",
                        "Ok");
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("Error", "Updating your GPS sent a bad request", "Ok");
                }
            }
        }

        private void UpdateStatus(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new UpdateStatusPage());
        }

        async void SendCheckIn(object sender, EventArgs e)
        {
            await SendMessage(MessageType.CheckIn);
        }

        private void SendRoutineMessage(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        async void SendUrgentAssistanceRequest(object sender, EventArgs e)
        {
            await SendMessage(MessageType.Urgent);
        }

        public async Task SendMessage(MessageType messageType)
        {
            try
            {
                MobileUserDto user = JsonConvert.DeserializeObject<MobileUserDto>(File.ReadAllText(FileHelper.UserFile));

                var url = UrlHelper.MessageUrl;
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ServerCertificateValidationCallback = delegate { return true; };
                webRequest.Method = "POST";
                webRequest.AllowAutoRedirect = false;
                webRequest.ContentType = "application/json";

                var message = new MessageDto
                {
                    SenderId = user.Id,
                    MessageType = messageType,
                    RecipientId = null,
                };

                if (messageType == MessageType.CheckIn)
                {
                    message.Content = $"{user.FirstName} {user.Surname} has checked in at {DateTime.Now}";
                }

                if (messageType == MessageType.Urgent)
                {
                    message.Content = $"{user.FirstName} {user.Surname} has requested immediate help at {DateTime.Now}";
                }


                var jsonString = JsonConvert.SerializeObject(message);
                webRequest.ContentLength = jsonString.Length;

                using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    await streamWriter.WriteAsync(jsonString);
                }


                var response = (HttpWebResponse)webRequest.GetResponse();

                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    if (messageType == MessageType.CheckIn)
                    {
                        await DisplayAlert("Check-In successful",
                            "Your buddy has been notified that you have checked in.", "Ok");
                    }

                    if (messageType == MessageType.Urgent)
                    {
                        await DisplayAlert("Assistance Requested",
                            "Your manager has been notified that you need immediate help.", "Ok");
                    }
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError && e.Response != null)
                {
                    var response = (HttpWebResponse)e.Response;
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        await DisplayAlert("Error", "A valid pairing could not be found to contact your buddy. Please inform your manager.", "Ok");

                    }

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        await DisplayAlert("Error", "Something went wrong when sending your message. Please try again.", "Ok");
                    }
                }
            }
        }
    }
}