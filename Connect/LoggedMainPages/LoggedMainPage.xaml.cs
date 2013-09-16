using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Connect.LoggedMainPages
{
    public partial class LoggedMainPage : PhoneApplicationPage
    {
        public LoggedMainPage()
        {
            InitializeComponent();
        }

        private void SettingsImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Settings.xaml", UriKind.Relative));
        }

        private void EnviarDatosLogin_Copy_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Browser.xaml", UriKind.Relative));
        }

        private void Click_Settings(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Settings.xaml", UriKind.Relative));
        }
    }
}