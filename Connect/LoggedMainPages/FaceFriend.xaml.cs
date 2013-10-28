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
    public partial class FaceFriend : PhoneApplicationPage
    {
        public FaceFriend()
        {
            InitializeComponent();
            LoggedUser lu = LoggedUser.Instance;
            this.loginBrowserControl.Navigate(new Uri("https://www.facebook.com/"+lu.friendInf.FacebookId, UriKind.Absolute));//aca va el face del nuevo amigo

        }
    }
}