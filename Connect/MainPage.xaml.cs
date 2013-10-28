using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Connect.Resources;
using System.IO;
using System.Windows.Threading;
using Newtonsoft.Json;
using Connect.Classes;
using Connect;
using System.Net.NetworkInformation;
using System.Windows.Media;

namespace Connect
{
    public partial class MainPage : PhoneApplicationPage
    {
        

        public static readonly DependencyProperty NetProperty = DependencyProperty.Register("NetworkAvailability", typeof(string), typeof(MainPage), new PropertyMetadata(string.Empty));

        public MainPage()
        {
            InitializeComponent();
            MailIngresado.Text = "prueba@mail.com";
            PassIngresado.Password = "password";
            // Código de ejemplo para traducir ApplicationBar
             BuildLocalizedApplicationBar();
            
      
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            while ((this.NavigationService.BackStack != null) && (this.NavigationService.BackStack.Any()))
            {
                this.NavigationService.RemoveBackEntry();
            } 
        }       

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }

        private void EnviarDatosLogin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Register.xaml", UriKind.Relative));
        }

        private void Click_check(object sender, EventArgs e)
        {
            //NavigationService.Navigate(new Uri("/LoggedMainPages/Scan.xaml", UriKind.Relative));
            try
            {
                if (MailIngresado.Text == "")
                {
                    ErrorBlock.Text = AppResources.invalidMail;
                    ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                }
                else
                    if (PassIngresado.Password == "")
                    {
                        ErrorBlock.Text = AppResources.invalidPassword;
                        ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {

                        ProgressB.IsIndeterminate = true;
                        ErrorBlock.Visibility = System.Windows.Visibility.Collapsed;
                        Connecting.Visibility = System.Windows.Visibility.Visible;
                        var webClient = new WebClient();
                        webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                        webClient.UploadStringCompleted += this.sendPostCompleted;

                        string json = "{\"Mail\":\"" + MailIngresado.Text + "\"," +
                                            "\"Password\":\"" + PassIngresado.Password + "\"}";

                        webClient.UploadStringAsync((new Uri(App.webService + "/api/Users/Login")), "POST", json);
                    }
                
            }
            catch (WebException webex)
            {
                HttpWebResponse webResp = (HttpWebResponse)webex.Response;

                switch (webResp.StatusCode)
                {
                    case HttpStatusCode.NotFound: // 404
                        break;
                    case HttpStatusCode.InternalServerError: // 500
                        break;
                    default:
                        break;
                }
            }
        }
        
        private void sendPostCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if ((e.Error!= null) && (e.Error.GetType().Name == "WebException"))
            {
                WebException we = (WebException)e.Error;
                HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;
                  
                switch (response.StatusCode)
                {
                    
                    case HttpStatusCode.NotFound: // 404
                        System.Diagnostics.Debug.WriteLine("Not found!");
                        ErrorBlock.Text = AppResources.WrongMailError;
                        ProgressB.IsIndeterminate = false;
                        Connecting.Visibility = System.Windows.Visibility.Collapsed;
                        ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");                        
                        ErrorBlock.Text = AppResources.WrongPasswordError;
                        ProgressB.IsIndeterminate = false;
                        Connecting.Visibility = System.Windows.Visibility.Collapsed;
                        ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                LoggedUser user = LoggedUser.Instance;
                user.SetLoggedUser(JsonConvert.DeserializeObject<UserData>(e.Result));
                ProgressB.IsIndeterminate = false;
                Connecting.Visibility = System.Windows.Visibility.Collapsed;
                ErrorBlock.Visibility = System.Windows.Visibility.Collapsed;                
                NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
            }
        }
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton =
                new ApplicationBarIconButton(new
                Uri("/Toolkit.Content/ApplicationBar.Check.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarLoginButtonText;
            appBarButton.Click += this.Click_check;
            ApplicationBar.Buttons.Add(appBarButton);
            ApplicationBar.BackgroundColor = Color.FromArgb(255, 0, 175, 240);
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBar.IsVisible = true;
            ApplicationBar.Opacity = (double)(.99);
            ApplicationBar.Mode = ApplicationBarMode.Default;

        }

    }
}