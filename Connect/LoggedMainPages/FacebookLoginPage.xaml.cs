using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Facebook.Client;
using System.Threading.Tasks;
using Connect.Classes;
using Facebook;

namespace Connect.LoggedMainPages
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {
        public FacebookLoginPage()
        {
            InitializeComponent();
            this.Loaded += FacebookLoginPage_Loaded;
        }

        async void FacebookLoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.isAuthenticated)
            {
                App.isAuthenticated = true;
                await Authenticate();
            }
        }

        private void LoadUserInfo()
        {
            var fb = new FacebookClient(App.AccessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }
                
                var result = (IDictionary<string, object>)e.GetResultData();

                Dispatcher.BeginInvoke(() =>
                {
                    LoggedUser user = LoggedUser.Instance;
                    UserData u = LoggedUser.Instance.userReg;
                    Session s = new Session();
                    s.RemoveStringObject("FacebookId");
                    s.SaveStringObject("FacebookId", (string)result["username"]);
                    u.FacebookId = (string)result["username"];
                });
            };

            fb.GetTaskAsync("me");
        }

        private FacebookSession session;

        private async Task Authenticate()
        {
            string message = String.Empty;
            try
            {
                session = await App.FacebookSessionClient.LoginAsync("user_about_me,read_stream");
                App.AccessToken = session.AccessToken;
                App.FacebookId = session.FacebookId;
                Session s = new Session();
                s.SaveStringObject("AccessToken", App.AccessToken);
                s.SaveStringObject("FacebookId", App.FacebookId);
                LoadUserInfo();
                Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/LoggedMainPages/Register2.xaml", UriKind.Relative)));
            }
            catch (InvalidOperationException e)
            {
                NavigationService.Navigate(new Uri("/LoggedMainPages/Register2.xaml", UriKind.Relative));
            }
        }
    }
}