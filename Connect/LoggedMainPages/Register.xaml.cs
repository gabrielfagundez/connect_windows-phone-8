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

namespace Connect.LoggedMainPages
{
    public partial class Register : PhoneApplicationPage
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Click_check(object sender, EventArgs e)
        {
            // NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
            try
            {
                
                ErrorBlockReg.Visibility = System.Windows.Visibility.Collapsed;
                var webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                webClient.UploadStringCompleted += this.sendPostCompleted;

                string json = "{\"Email\":\"" + MailIngresado.Text + "\"," +
                                  "\"Password\":\"" + "est" + "\"}";

                if (NombreIngresado.Text == "")
                {
                    ErrorBlockReg.Text = "Please write a name";
                    ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                }
                else
                if (MailIngresado.Text == "")
                {
                    ErrorBlockReg.Text = "Please write an email address";
                    ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                }
                else
                if (!IsValidEmail(MailIngresado.Text))
                {
                    ErrorBlockReg.Text = "That email address is not valid";
                    ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                }
                else
                if (PassIngresadoReg.Password == "")
                {
                    ErrorBlockReg.Text = "Please write a password";
                    ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                }
                else
                if (PassIngresadoReg.Password.Length < 6 )
                {
                    ErrorBlockReg.Text = "The password should at least be 6 characters long";
                    ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                }
                else
                if (PassIngresadoReg.Password != RePassIngresadoReg.Password)
                {
                    ErrorBlockReg.Text = "Written passwords are not the same";
                    ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    ProgressB.IsIndeterminate = true;
                    Connecting.Visibility = System.Windows.Visibility.Visible;
                    webClient.UploadStringAsync((new Uri("http://servidorpis.azurewebsites.net/api/login/")), "POST", json);
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
                        LoggedUser u= LoggedUser.Instance;
                        UserData user = u.RegisterUser();
                        user.Name = NombreIngresado.Text;
                        user.Email = MailIngresado.Text;
                        user.Password = PassIngresadoReg.Password;
                        user.FacebookId = "not connected";
                        user.LinkedInId = "not connected";
                        user.Id = "";
                        u.SetLoggedUser(user);
                        NavigationService.Navigate(new Uri("/LoggedMainPages/Register2.xaml", UriKind.Relative));          
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("ya fue usado el mail!");
                        ErrorBlockReg.Text = "That mail has already been used";
                        ProgressB.IsIndeterminate = false;
                        Connecting.Visibility = System.Windows.Visibility.Collapsed;
                        ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
        }        
    }
}