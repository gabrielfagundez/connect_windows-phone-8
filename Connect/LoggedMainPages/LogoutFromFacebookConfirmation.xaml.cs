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
using Facebook.Client;

namespace Connect.LoggedMainPages
{
    public partial class LogoutFromFacebookConfirmation : PhoneApplicationPage
    {
        public LogoutFromFacebookConfirmation()
        {
            InitializeComponent();
        }

        private async void Yes_Click(object sender, RoutedEventArgs e)
        {
            LoggedUser.Instance.userReg.FacebookId = "";            
            FacebookSessionCacheProvider.Current.DeleteSessionData();
            await new WebBrowser().ClearCookiesAsync();
            App.isAuthenticated = false;
            NavigationService.Navigate(new Uri("/LoggedMainPages/Register2.xaml", UriKind.Relative));
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Register2.xaml", UriKind.Relative));
        }
    }
}