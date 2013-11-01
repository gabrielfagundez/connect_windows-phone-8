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

namespace Connect.LoggedMainPages
{
    public partial class LogoutFromLinkedInConfirmation : PhoneApplicationPage
    {
        public LogoutFromLinkedInConfirmation()
        {
            InitializeComponent();
        }

        private async void Yes_Click(object sender, RoutedEventArgs e)
        {
            LoggedUser.Instance.userReg.LinkedInId = "";
            await new WebBrowser().ClearCookiesAsync();
            MainUtil.SetKeyValue<string>("AccessToken", string.Empty);
            MainUtil.SetKeyValue<string>("AccessTokenSecret", string.Empty);
            NavigationService.Navigate(new Uri("/LoggedMainPages/Register2.xaml", UriKind.Relative));
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Register2.xaml", UriKind.Relative));
        }
    }
}