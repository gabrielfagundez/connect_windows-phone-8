using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Connect.Classes;
using System.Net.NetworkInformation;

namespace Connect.LoggedMainPages
{
    public partial class LinkFriend : PhoneApplicationPage
    {
        public LinkFriend()
        {
            InitializeComponent();
            this.Loaded += LinkedinPage_Loaded;
        }

        async void LinkedinPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoggedUser lu = LoggedUser.Instance;
            string s = lu.friendInf.LinkedInId;
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(s);
            webBrowserTask.Show();
            NavigationService.Navigate(new Uri("/LoggedMainPages/FriendInfo.xaml", UriKind.Relative));
        }    
         
    }
}