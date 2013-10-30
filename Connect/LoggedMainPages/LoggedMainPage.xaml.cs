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
    public partial class LoggedMainPage : PhoneApplicationPage
    {
        public LoggedMainPage()
        {
            InitializeComponent();
            // Código de ejemplo para traducir ApplicationBar
            BuildLocalizedApplicationBar();
            LoggedUser user = LoggedUser.Instance;
            string codigo = user.GetLoggedUser().Id ; 
            var writer = new ZXing.BarcodeWriter { Format = ZXing.BarcodeFormat.QR_CODE };
            var writeableBitmap = writer.Write(codigo);
            ImgQR.Stretch = System.Windows.Media.Stretch.Fill;
            ImgQR.Source = writeableBitmap;
            UsName.Text = "Welcome " + user.GetLoggedUser().Name;
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            while ((this.NavigationService.BackStack != null) && (this.NavigationService.BackStack.Any()))
            {
                this.NavigationService.RemoveBackEntry();
            }
        }

        private void Click_Settings(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Settings.xaml", UriKind.Relative));
        }

        private void Rectangle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Scan.xaml", UriKind.Relative));
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Scan.xaml", UriKind.Relative));
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Scan.xaml", UriKind.Relative));
        }
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton =
                new ApplicationBarIconButton(new
                Uri("/Assets/Images/feature.settings.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarSettingsButtonText;
            appBarButton.Click += this.Click_Settings;
            ApplicationBar.Buttons.Add(appBarButton);
            ApplicationBar.BackgroundColor = Color.FromArgb(255, 0, 175, 240);
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBar.IsVisible = true;
            ApplicationBar.Opacity = (double)(.99);
            ApplicationBar.Mode = ApplicationBarMode.Default;

        }
    }
}