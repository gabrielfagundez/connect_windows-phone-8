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
    public partial class Done : PhoneApplicationPage
    {
        public Done()
        {
            InitializeComponent();
        }

        private void Click_check(object sender, EventArgs e)
        {
            //**esto es para testing***
            LoggedUser lu = LoggedUser.Instance;
            lu.friendInf = new UserData();
            lu.friendInf.Name = "pepito";
            lu.friendInf.Mail = "pepito@mail.com";
            lu.friendInf.FacebookId = "";
            lu.friendInf.LinkedInId = "";
            //**************************
            NavigationService.Navigate(new Uri("/LoggedMainPages/FriendInfo.xaml", UriKind.Relative));
        }
    }
}