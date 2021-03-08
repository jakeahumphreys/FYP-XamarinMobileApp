using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FYP_Mobile.Common;
using FYP_Mobile.DTO;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FYP_Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendRoutineMessage : ContentPage
    {
        private MobileUserDto _user { get; set; }

        public SendRoutineMessage(MobileUserDto user)
        {
            InitializeComponent();
            _user = user;
        }

        async void SubmitMessage(object sender, EventArgs e)
        {
            await SendMessage(MessageText.Text);
        }

        async Task SendMessage(string messageText)
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
                    MessageType = MessageType.Routine,
                    RecipientId = _user.Id,
                    Content = messageText
                };

                var jsonString = JsonConvert.SerializeObject(message);
                webRequest.ContentLength = jsonString.Length;

                using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    await streamWriter.WriteAsync(jsonString);
                }


                var response = (HttpWebResponse)webRequest.GetResponse();

                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    await DisplayAlert("Message Sent",
                        "Your message has been sent successfully.", "Ok");

                    App.Current.MainPage = new NavigationPage(new ApplicationHome());
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError && e.Response != null)
                {
                    var response = (HttpWebResponse)e.Response;

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        await DisplayAlert("Error", "Something went wrong when sending your message. Please try again.", "Ok");
                    }
                }
            }
        }
    }
}