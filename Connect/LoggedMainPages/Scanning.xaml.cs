using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;

namespace Connect.LoggedMainPages
{
    public partial class Scanning : PhoneApplicationPage
    {
        private HttpWebRequest request;

        public Scanning()
        {
            InitializeComponent();
            Loaded += SplashPage_Loaded;
        }


        void SplashPage_Loaded(object sender, RoutedEventArgs e)
        {
            request = (HttpWebRequest)HttpWebRequest.Create(new Uri("http://www.developer.nokia.com/Community/Wiki/Portal:Windows_Phone_UI_Articles"));
            request.BeginGetResponse(new AsyncCallback(ReceiveResponseCallBack), null);
        }

        private void ReceiveResponseCallBack(IAsyncResult result)
        {
            HttpWebResponse response = (HttpWebResponse)this.request.EndGetResponse(result);
            using (var stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string str = reader.ReadToEnd();
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        NavigationService.Navigate(new Uri("/LoggedMainPages/Done.xaml", UriKind.Relative));
                    });
                }
            }
        }
    }
}