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


namespace Connect.LoggedMainPages
{
    public partial class Register2 : PhoneApplicationPage
    {
       

        public Register2()
        {
            InitializeComponent();
        }

        private void btnFacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/FacebookLoginPage.xaml", UriKind.Relative));
        }

        private void LoadUserInfo()
        {
            var fb = new FacebookClient(App.AccessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();

                Dispatcher.BeginInvoke(() =>
                {
                    LoggedUser user = LoggedUser.Instance;
                    UserData u = user.GetLoggedUser();
                    u.FacebookId = (string)result["username"];
                    user.SetLoggedUser(u);
                });
            };

            fb.GetTaskAsync("me");
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
                                "\"Email\":\"" + u.Email + "\"," +
                                 "\"FacebookId\":\"" + u.FacebookId + "\"," +
                                  "\"LinkedInId\":\"" + u.LinkedInId + "\"," +
                                  "\"Password\":\"" + u.Password + "\"}";
                System.Diagnostics.Debug.WriteLine(json);

                webClient.UploadStringAsync((new Uri("http://connectwp.azurewebsites.net/api/SignUp/")), "POST", json);
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
                if (App.AccessToken != "")
                {
                    LoadUserInfo();
                }
                LoggedUser user = LoggedUser.Instance;
                user.SetLoggedUser(JsonConvert.DeserializeObject<UserData>(e.Result));
                NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
            }
        }

        

  
    }
}