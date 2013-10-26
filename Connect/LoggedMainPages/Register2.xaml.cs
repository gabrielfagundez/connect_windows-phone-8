﻿using System;
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

namespace Connect.LoggedMainPages
{
    public partial class Register2 : PhoneApplicationPage
    {

        string OAuthTokenKey = string.Empty;
        string tokenSecret = string.Empty;
        string accessToken = string.Empty;
        string accessTokenSecret = string.Empty;

        XmlSerializer personXerializer;
        person currentPerson;

        
        public Register2()
        {
            InitializeComponent();
            btnFacebookLogin.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnFacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            btnFacebookLogin.Visibility = System.Windows.Visibility.Collapsed;
            App.registrando = true;
            NavigationService.Navigate(new Uri("/LoggedMainPages/FacebookLoginPage.xaml", UriKind.Relative));
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (App.registrando == false)
            {
                LoggedUser user = LoggedUser.Instance;
                await user.LogOut();
            }
            else
            {
                App.registrando = false;
            }
            
        }



        private void Click_check(object sender, EventArgs e)
        {
            // NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
            try
            {                
                var webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                webClient.UploadStringCompleted += this.sendPostCompleted;
                LoggedUser user = LoggedUser.Instance;
                UserData u = user.GetLoggedUser();
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
                App.registrando = true;
                NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
            }
        }

        private void MenuItemSignIn_Click(object sender, EventArgs e)
        {
            App.registrando = true;
            NavigationService.Navigate(new Uri("/LoggedMainPages/Linkedin.xaml", UriKind.Relative));
        }

    }
}