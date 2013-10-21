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

namespace Connect
{
    public partial class MainPage : PhoneApplicationPage
    {
        

        public static readonly DependencyProperty NetProperty = DependencyProperty.Register("NetworkAvailability", typeof(string), typeof(MainPage), new PropertyMetadata(string.Empty));

        public MainPage()
        {
            InitializeComponent();
            
      
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
            // NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
            try
            {
                if (MailIngresado.Text == "")
                {
                    ErrorBlock.Text = "Please write an email address";
                    ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                }
                else
                    if (PassIngresado.Password == "")
                    {
                        ErrorBlock.Text = "Please write a password";
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

                        webClient.UploadStringAsync((new Uri("http://testingpis.azurewebsites.net/api/Users/Login")), "POST", json);
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
                        ErrorBlock.Text = "That email is not registered";
                        ProgressB.IsIndeterminate = false;
                        Connecting.Visibility = System.Windows.Visibility.Collapsed;
                        ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");                        
                        ErrorBlock.Text = "The password is not correct";
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



    }
}