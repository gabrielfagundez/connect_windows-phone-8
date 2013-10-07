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
    public partial class LogoutConfirmation : PhoneApplicationPage
    {
        public LogoutConfirmation()
        {
            InitializeComponent();
        }

        private async void Yes_Click(object sender, RoutedEventArgs e)
        {
            LoggedUser l = LoggedUser.Instance;
            await l.LogOut();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Settings.xaml", UriKind.Relative));
        }
    }
}