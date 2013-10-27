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
using System.IO;
using System.Windows.Threading;
using Newtonsoft.Json;
using Connect;
using Facebook;

namespace Connect.LoggedMainPages
{
    public partial class Settings : PhoneApplicationPage
    {
        public Settings()
        {
            InitializeComponent();

           LoggedUser user = LoggedUser.Instance;
           UserData _userData = user.GetLoggedUser();

            MailLable.Text =  _userData.Mail;
            MailLable.IsReadOnly = true;

            NameInputLable.Text =  _userData.Name;
            NameInputLable.IsReadOnly = true;

            



            MailAccountBlock.Text = _userData.Mail;
            if (_userData.FacebookId == "")
            {
                FacebookAccountBlock.Text = "not connected";
            }
            else
            {
                FacebookAccountBlock.Text = _userData.FacebookId;
            }

            if ((_userData.LinkedInId == "") || (_userData.LinkedInId == null))
            {
                LinkedInAccountBlock.Text = "not connected";
            }
            else
            {
                LinkedInAccountBlock.Text = _userData.LinkedInId;
            }
            
        }

        

        private void Click_check(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));            
        }

        private void Click_Logout(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/LogoutConfirmation.xaml", UriKind.Relative));
        }



        private void TextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Leng.Text == "Español")
                Leng.Text = "English";
            else
                Leng.Text = "Español";
        }

    }     
}