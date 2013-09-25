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




namespace Connect
{
    public partial class MainPage : PhoneApplicationPage
    {
        static private Uri usuario = new Uri("http://connectwp.azurewebsites.net/api/Users/10");       
        private UserData UsuarioLogin;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;         
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(usuario); 
            
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }



        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            UsuarioLogin = JsonConvert.DeserializeObject<UserData>(e.Result);        
            MailIngresado.Text = UsuarioLogin.Email;
            PassIngresado.Password = UsuarioLogin.Password;
        }

       

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }

        private void EnviarDatosLogin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Register.xaml", UriKind.Relative));
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
                        ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");                        
                        ErrorBlock.Text = "The password is not correct";
                        ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                UsuarioLogin = JsonConvert.DeserializeObject<UserData>(e.Result);
                ErrorBlock.Visibility = System.Windows.Visibility.Collapsed;
                NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
            }
        }


        private void Click_check(object sender, EventArgs e)        
        {
            try
            {
                var webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                webClient.UploadStringCompleted += this.sendPostCompleted;

                string json = "{\"Email\":\"" + MailIngresado.Text +  "\"," +
                                  "\"Password\":\"" + PassIngresado.Password + "\"}";

                webClient.UploadStringAsync((new Uri("http://connectwp.azurewebsites.net/api/login/")), "POST", json);
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

   

        

      

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}