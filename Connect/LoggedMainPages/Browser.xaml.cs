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
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
            string site = "http://mepaseaste.uy";
            Browserfb.Navigate(new Uri(site, UriKind.Absolute));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            string site = "https://www.facebook.com/dialog/friends/?id=bicicletas.ikatch&app_id=575468829178063&redirect_uri=http://mepaseaste.uy/";
            Browserfb.Navigate(new Uri(site, UriKind.Absolute));
        }

        private void Browserfb_Navigating(object sender, NavigatingEventArgs e)
        {
            string hola =e.Uri.Host + e.Uri.AbsolutePath;
            friendName.Text = hola;
            if (e.Uri == e.Uri)
            {
                e.Cancel = true;
                NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
                //Send friend request succcess
                //....
            }
        }

       
    }
}