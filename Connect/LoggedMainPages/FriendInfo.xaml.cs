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
    public partial class FriendInfo : PhoneApplicationPage
    {
        public FriendInfo()
        {
            InitializeComponent();
            LoggedUser lu = LoggedUser.Instance;
            NameInputLable.Text=lu.friendInf.Name;
            MailLable.Text = lu.friendInf.Mail;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
        }

        private void btnFacebook_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/FaceFriend.xaml", UriKind.Relative));

        }

        private void btnLinkedin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/LinkFriend.xaml", UriKind.Relative));

        }

    }
}