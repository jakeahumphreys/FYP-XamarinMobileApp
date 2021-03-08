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
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FYP_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateStatusPage : ContentPage
    {
        private CancellationTokenSource cts;

        public UpdateStatusPage()
        {
            InitializeComponent();
        }

        async void SetNotOnShift(object sender, EventArgs e)
        {
            await UpdateLocationAsync(Status.NotOnShift);
        }

        async void SetOnShiftNotMobile(object sender, EventArgs e)
        {
            await UpdateLocationAsync(Status.OnShiftNotMobile);
        }

        async void SetTransitToLocation(object sender, EventArgs e)
        {
            await UpdateLocationAsync(Status.TransitToLocation);
        }

        async void SetAtLocation(object sender, EventArgs e)
        {
            await UpdateLocationAsync(Status.AtLocation);
        }

        async void SetBreak(object sender, EventArgs e)
        {
            await UpdateLocationAsync(Status.Break);
        }

        public async Task UpdateLocationAsync(Status status)
        {
            try
            {
                MobileUserDto user = JsonConvert.DeserializeObject<MobileUserDto>(File.ReadAllText(FileHelper.UserFile));

                var url = UrlHelper.StatusUrl;
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ServerCertificateValidationCallback = delegate { return true; };
                webRequest.Method = "POST";
                webRequest.AllowAutoRedirect = false;
                webRequest.ContentType = "application/json";

                var update = new StatusUpdate
                {
                    status = status,
                    UserId = user.Id
                };

                var jsonString = JsonConvert.SerializeObject(update);
                webRequest.ContentLength = jsonString.Length;

                using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    await streamWriter.WriteAsync(jsonString);
                }


                var response = (HttpWebResponse)webRequest.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (status != Status.NotOnShift)
                    {
                        await SendGpsReport(user.Id);
                    }
                    else
                    {
                        await DisplayAlert("Booked off", "Your status is now not on shift, to protect your privacy, no location has been sent.", "Ok");
                    }
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("Error", "Updating your status sent a bad request", "Ok");
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError && e.Response != null)
                {
                    var response = (HttpWebResponse)e.Response;
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        await DisplayAlert("Invalid Credentials",
                            "The details you entered were not correct, please use the details provided by your line manager.", "Ok");
                    }

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        await DisplayAlert("Invalid Data", e.Message, "Ok");
                    }
                }
            }


        }

        public async Task SendGpsReport(string userId)
        {
            try
            {
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
                        UserId = userId
                    };

                    var url = UrlHelper.GpsReportUrl;
                    var webRequest = (HttpWebRequest) WebRequest.Create(url);
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


                    var response = (HttpWebResponse) webRequest.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        await DisplayAlert("Status Updated", "Your updated status has been logged successfully", "Ok");
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
                        await DisplayAlert("Invalid Credentials",
                            "The details you entered were not correct, please use the details provided by your line manager.", "Ok");
                    }

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        await DisplayAlert("Invalid Data", e.Message, "Ok");
                    }
                }
            }

        }
    }
}