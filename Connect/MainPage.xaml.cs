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

namespace Connect
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }

        private void EnviarDatosLogin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Register.xaml", UriKind.Relative));
        }


        private void Click_check(object sender, EventArgs e)
        {
            if (((MailIngresado.Text == "guille") && (PassIngresado.Password == "pass")) || ((MailIngresado.Text == "") && (PassIngresado.Password == "")))
            {
                ErrorBlock.Visibility = System.Windows.Visibility.Collapsed;
                NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
            }
            else
            {
                ErrorBlock.Visibility = System.Windows.Visibility.Visible;
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