using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Connect.Classes;
using Connect.Resources;
using System.IO;
using System.Windows.Threading;
using Newtonsoft.Json;
using Connect;
using Facebook;
using Connect.Resources;
using System.Windows.Media;

namespace Connect.LoggedMainPages
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();
            // Código de ejemplo para traducir ApplicationBar
            BuildLocalizedApplicationBar();

           LoggedUser user = LoggedUser.Instance;
           UserData _userData = user.GetLoggedUser();

            MailLable.Text =  _userData.Mail;
            MailLable.IsReadOnly = true;

            NameInputLable.Text =  _userData.Name;
            NameInputLable.IsReadOnly = true;

            



            MailAccountBlock.Text = _userData.Mail;
            if (_userData.FacebookId == "")
            {
                FacebookAccountBlock.Text = "not connected";
            }
            else
            {
                FacebookAccountBlock.Text = _userData.FacebookId;
            }

            if ((_userData.LinkedInId == "") || (_userData.LinkedInId == null))
            {
                LinkedInAccountBlock.Text = "not connected";
            }
            else
            {
                LinkedInAccountBlock.Text = _userData.LinkedInId;
            }
            
        }

        

     

        private void Click_Logout(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/LogoutConfirmation.xaml", UriKind.Relative));
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton =
                new ApplicationBarIconButton(new
                Uri("/Assets/Images/Logout-32.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarLogoutButtonText;
            appBarButton.Click += this.Click_Logout;
            ApplicationBar.Buttons.Add(appBarButton);
            ApplicationBar.BackgroundColor = Color.FromArgb(255, 0, 175, 240);
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBar.IsVisible = true;
            ApplicationBar.Opacity = (double)(.99);
            ApplicationBar.Mode = ApplicationBarMode.Default;
        }


      

    }     
}