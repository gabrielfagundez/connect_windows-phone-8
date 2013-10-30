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
using System.Windows.Media;

namespace Connect.LoggedMainPages
{
    public partial class FriendInfo : PhoneApplicationPage
    {
        public FriendInfo()
        {
            InitializeComponent();
            // Código de ejemplo para traducir ApplicationBar
            BuildLocalizedApplicationBar();
            LoggedUser lu = LoggedUser.Instance;

            Done.Text = lu.friendInf.Name + ", con el email " + lu.friendInf.Mail + " acaba de conectarse contigo!";
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
        }

        private void btnFacebook_Click(object sender, RoutedEventArgs e)
        {
            LoggedUser lu = LoggedUser.Instance;
            if(lu.friendInf.FacebookId!="")
            {
                NavigationService.Navigate(new Uri("/LoggedMainPages/FaceFriend.xaml", UriKind.Relative));
            }
            else
            {

                SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));    
                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = "",
                    Message = "El usuario no tiene cuenta de Facebook ascociada",
                    LeftButtonContent = "Aceptar",
                    
                    Background = mybrush,
                    IsFullScreen = false
                };
                messageBox.Show();
            }
            

        }

        private void btnLinkedin_Click(object sender, RoutedEventArgs e)
        {
            LoggedUser lu = LoggedUser.Instance;
            if (lu.friendInf.FacebookId != "")
            {
                NavigationService.Navigate(new Uri("/LoggedMainPages/LinkFriend.xaml", UriKind.Relative));
            }
            else
            {

                SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = "",
                    Message = "El usuario no tiene cuenta de Linkedin ascociada",
                    LeftButtonContent = "Aceptar",

                    Background = mybrush,
                    IsFullScreen = false
                };
                messageBox.Show();
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
            appBarButton.Click += this.ApplicationBarIconButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);
            ApplicationBar.BackgroundColor = Color.FromArgb(255, 0, 175, 240);
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBar.IsVisible = true;
            ApplicationBar.Opacity = (double)(.99);
            ApplicationBar.Mode = ApplicationBarMode.Default;

        }

    }
}