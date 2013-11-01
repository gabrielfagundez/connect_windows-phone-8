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
using System.Text.RegularExpressions;
using System.Windows.Media;
using Microsoft.Phone.Net.NetworkInformation;
using Facebook.Client;

namespace Connect.LoggedMainPages
{
    public partial class Register : PhoneApplicationPage
    {
        public Register()
        {
            InitializeComponent();
            // Código de ejemplo para traducir ApplicationBar
            BuildLocalizedApplicationBar();
            NombreIngresado.Text = "Prueba Perez";
            MailIngresado.Text = "prueba@mail.com";                    
            PassIngresadoReg.Password = "password";
            RePassIngresadoReg.Password = "password";
                              
                                  
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {           
                //LoggedUser.Instance.userReg = null ;          
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
                    if (App.FacebookSessionClient == null)
                    {
                        App.FacebookSessionClient = new FacebookSessionClient(Constants.FacebookAppId);
                    }
                    ErrorBlockReg.Visibility = System.Windows.Visibility.Collapsed;
                    var webClient = new WebClient();
                    webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                    webClient.UploadStringCompleted += this.sendPostCompleted;

                    string json = "{\"Mail\":\"" + MailIngresado.Text + "\"," +
                                      "\"Password\":\"" + "est" + "\"}";

                    if (NombreIngresado.Text == "")
                    {
                        ErrorBlockReg.Text = AppResources.invalidName;
                        ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                        if (MailIngresado.Text == "")
                        {
                            ErrorBlockReg.Text = AppResources.invalidMail;
                            ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                            if (!IsValidEmail(MailIngresado.Text))
                            {
                                ErrorBlockReg.Text = AppResources.WrongMailError;
                                ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                                if (PassIngresadoReg.Password == "")
                                {
                                    ErrorBlockReg.Text = AppResources.invalidPassword;
                                    ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                                }
                                else
                                    if (PassIngresadoReg.Password.Length < 6)
                                    {
                                        ErrorBlockReg.Text = AppResources.invalidPassword1;
                                        ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                                    }
                                    else
                                        if (PassIngresadoReg.Password != RePassIngresadoReg.Password)
                                        {
                                            ErrorBlockReg.Text = AppResources.invalidReType;
                                            ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                                        }
                                        else
                                        {
                                            ProgressB.IsIndeterminate = true;
                                            Connecting.Visibility = System.Windows.Visibility.Visible;
                                            webClient.UploadStringAsync((new Uri(App.webService + "/api/Users/Login/")), "POST", json);
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

            
        

    public static bool IsValidEmail(string strIn)
    {
        // Return true if strIn is in valid e-mail format.
        return Regex.IsMatch(strIn, 
                @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" + 
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"); 
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
                        System.Diagnostics.Debug.WriteLine("no esta usado el mail!");  
                        ProgressB.IsIndeterminate = false;
                        Connecting.Visibility = System.Windows.Visibility.Collapsed;
                        ErrorBlockReg.Visibility = System.Windows.Visibility.Collapsed;
                       
                        LoggedUser.Instance.userReg.Name = NombreIngresado.Text;
                        LoggedUser.Instance.userReg.Mail = MailIngresado.Text;
                        LoggedUser.Instance.userReg.Password = PassIngresadoReg.Password;
                        LoggedUser.Instance.userReg.FacebookId = "";
                        LoggedUser.Instance.userReg.LinkedInId = "";
                        LoggedUser.Instance.userReg.Id = "";
                        NavigationService.Navigate(new Uri("/LoggedMainPages/Register2.xaml", UriKind.Relative));          
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("ya fue usado el mail!");
                        ErrorBlockReg.Text = AppResources.alreadyUsedEmail;
                        ProgressB.IsIndeterminate = false;
                        Connecting.Visibility = System.Windows.Visibility.Collapsed;
                        ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                        break;
                    default:
                        break;
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