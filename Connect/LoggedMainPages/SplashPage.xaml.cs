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
using System.Threading;
using Microsoft.Phone.Controls;
using System.Windows.Threading;
using Connect.Classes;

namespace WindowsPhoneSample5
{
    public partial class SplashPage : PhoneApplicationPage
    {
        private HttpWebRequest request;
        private int i = 0;
        private Uri a;

        public SplashPage()
        {
            InitializeComponent();
            // creating timer instance
            DispatcherTimer newTimer = new DispatcherTimer();

            UserData user = LoggedUser.Instance.GetLoggedUser();
            if (user == null)
            {
                a = new Uri("/MainPage.xaml", UriKind.Relative);
            }
            else
            {
                a = new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative);
            }
            // timer interval specified as 1 second
            newTimer.Interval = TimeSpan.FromSeconds(1);
            // Sub-routine OnTimerTick will be called at every 1 second
            newTimer.Tick += OnTimerTick;
            // starting the timer
            newTimer.Start();
        }

        void OnTimerTick(Object sender, EventArgs args)
        {
            // text box property is set to current system date.
            // ToString() converts the datetime value into text
            i += 1;
            if (i == 1)
            {
                NavigationService.Navigate(a);
            }
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
                        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    });
                }
            }
        }
    }
}