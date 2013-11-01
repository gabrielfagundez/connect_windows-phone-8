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
using Facebook;
using System.Xml.Serialization;
using System.Text;
using Hammock.Web;
using Hammock;
using Hammock.Authentication.OAuth;
using System.Windows.Media;
using System.Net.NetworkInformation;
using Microsoft.Phone.Net.NetworkInformation;
using Facebook.Client;

namespace Connect.LoggedMainPages
{
    public partial class Register2 : PhoneApplicationPage
    {

        string OAuthTokenKey = string.Empty;
        string tokenSecret = string.Empty;
        string accessToken = string.Empty;
        string accessTokenSecret = string.Empty;
        
        public Register2()
        {
            InitializeComponent();
            // Código de ejemplo para traducir ApplicationBar
            BuildLocalizedApplicationBar();
           
        }

        private void btnFacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedUser.Instance.userReg.FacebookId != "")
            {
                NavigationService.Navigate(new Uri("/LoggedMainPages/LogoutFromFacebookConfirmation.xaml", UriKind.Relative));
            }
            else
            {
                if (IsNetworkAvailable())
                {

                    NavigationService.Navigate(new Uri("/LoggedMainPages/FacebookLoginPage.xaml", UriKind.Relative));
                }
                else
                {
                    SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
                    CustomMessageBox messageBox = new CustomMessageBox()
                    {
                        Caption = AppResources.NoInternetConnection,
                        Message = AppResources.NoInternetConnectionMessage,
                        LeftButtonContent = AppResources.OkTitle,
                        Background = mybrush,
                        IsFullScreen = false,
                    };


                    messageBox.Dismissed += (s1, e1) =>
                    {
                        switch (e1.Result)
                        {
                            case CustomMessageBoxResult.LeftButton:
                                break;
                            case CustomMessageBoxResult.None:
                                // Acción.
                                break;
                            default:
                                break;
                        }
                    };

                    messageBox.Show();
                }
            }
        }

        protected async override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            LoggedUser.Instance.userReg.FacebookId = "";            
            FacebookSessionCacheProvider.Current.DeleteSessionData();
            await new WebBrowser().ClearCookiesAsync();
            App.isAuthenticated = false;
            LoggedUser.Instance.userReg.LinkedInId = "";
            MainUtil.SetKeyValue<string>("AccessToken", string.Empty);
            MainUtil.SetKeyValue<string>("AccessTokenSecret", string.Empty);
            NavigationService.Navigate(new Uri("/LoggedMainPages/Register.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (LoggedUser.Instance.userReg.FacebookId != "")
            {
                btnFacebookLogin.Content = AppResources.LogoutFromFacebookTitle;
            }
            if (LoggedUser.Instance.userReg.LinkedInId != "")
            {
                btnLinkedinLogin.Content = AppResources.LogoutFromLinkedInTitle;
            }
                //LoggedUser.Instance.userReg = null;
           
        }
        
        private bool IsNetworkAvailable()
        {
            if (App.isDebug)
                return false;
            else if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
                return false;
            else
                return true;
        }




        private void Click_check(object sender, EventArgs e)
        {
           
            if (IsNetworkAvailable())
            {
                try
                {
                    var webClient = new WebClient();
                    webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                    webClient.UploadStringCompleted += this.sendPostCompleted;
                    UserData u = LoggedUser.Instance.userReg;
                    string json = "{\"Name\":\"" + u.Name + "\"," +
                                    "\"Mail\":\"" + u.Mail + "\"," +
                                     "\"FacebookId\":\"" + u.FacebookId + "\"," +
                                      "\"LinkedInId\":\"" + u.LinkedInId + "\"," +
                                      "\"Password\":\"" + u.Password + "\"}";
                    System.Diagnostics.Debug.WriteLine(json);

                    webClient.UploadStringAsync((new Uri(App.webService + "/api/Users/SignUp/")), "POST", json);
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
            else
            {
                SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = AppResources.NoInternetConnection,
                    Message = AppResources.NoInternetConnectionMessageRegister,
                    LeftButtonContent = AppResources.OkTitle,
                    Background = mybrush,
                    IsFullScreen = false,
                };


                messageBox.Dismissed += (s1, e1) =>
                {
                    switch (e1.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
                            break;
                        case CustomMessageBoxResult.None:
                            // Acción.
                            break;
                        default:
                            break;
                    }
                };

                messageBox.Show();
            }
        }

        private void sendPostCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if ((e.Error != null) && (e.Error.GetType().Name == "WebException"))
            {
                WebException we = (WebException)e.Error;
                HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                switch (response.StatusCode)
                {

                    case HttpStatusCode.NotFound: // 404
                        System.Diagnostics.Debug.WriteLine("Not found!");
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");
                        break;
                    default:
                        break;
                }
            }
            else
            {                
                LoggedUser user = LoggedUser.Instance;
                user.SetLoggedUser(JsonConvert.DeserializeObject<UserData>(e.Result));
                MainUtil.SetKeyValue<string>("AccessToken", string.Empty);
                MainUtil.SetKeyValue<string>("AccessTokenSecret", string.Empty);
                NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
            }
        }



        private void MenuItemSignIn_Click(object sender, EventArgs e)
        {
            if (LoggedUser.Instance.userReg.LinkedInId != "")
            {
                NavigationService.Navigate(new Uri("/LoggedMainPages/LogoutFromLinkedInConfirmation.xaml", UriKind.Relative));
            }
            else
            {
                if (IsNetworkAvailable())
                {
                    NavigationService.Navigate(new Uri("/LoggedMainPages/Linkedin.xaml", UriKind.Relative));
                }
                else
                {
                    SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
                    CustomMessageBox messageBox = new CustomMessageBox()
                    {
                        Caption = AppResources.NoInternetConnection,
                        Message = AppResources.NoInternetConnectionMessage,
                        LeftButtonContent = AppResources.OkTitle,
                        Background = mybrush,
                        IsFullScreen = false,
                    };


                    messageBox.Dismissed += (s1, e1) =>
                    {
                        switch (e1.Result)
                        {
                            case CustomMessageBoxResult.LeftButton:
                                break;
                            case CustomMessageBoxResult.None:
                                // Acción.
                                break;
                            default:
                                break;
                        }
                    };

                    messageBox.Show();
                }
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
            appBarButton.Text = AppResources.AppBarDoneButtonText;
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