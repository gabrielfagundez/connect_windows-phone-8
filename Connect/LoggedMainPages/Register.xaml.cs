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
    public partial class Register : PhoneApplicationPage
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Click_check(object sender, EventArgs e)
        {
            if ((MailIngresado.Text != "") && (PassIngresadoReg.Password == RePassIngresadoReg.Password))
            {
                ErrorBlockReg.Visibility = System.Windows.Visibility.Collapsed; 
                NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));

            }
            else
            {
                if (MailIngresado.Text == "")
                {
                    ErrorBlockReg.Text = "Please write a mail address";
                    ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    ErrorBlockReg.Text = "Written passwords are not the same";
                    ErrorBlockReg.Visibility = System.Windows.Visibility.Visible;
                }


            }
        }
    }
}