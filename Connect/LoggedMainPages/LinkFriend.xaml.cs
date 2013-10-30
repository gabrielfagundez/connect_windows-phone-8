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
            LoggedUser lu = LoggedUser.Instance;
            string s = lu.friendInf.LinkedInId;


            int startPos = 0;
            int length = s.IndexOf("&amp;authType=") - startPos;
            string id = s.Substring(startPos, length);
     

            startPos = s.LastIndexOf("&amp;authType=") + "&amp;authType=".Length;
            length = s.IndexOf("&amp;authToken=") - startPos;
            string authType = s.Substring(startPos, length);
       

            startPos = s.LastIndexOf("&amp;authToken=") + "&amp;authToken=".Length;
            length = s.Length - startPos;
            string authToken = s.Substring(startPos, length);
       
            this.loginBrowserControl.Navigate(new Uri("http://www.linkedin.com/profile/view?id=" + id + "&authType=" + authType + "NAME_SEARCH&authToken=" + authToken, UriKind.Absolute));//aca va el linkedin del nuevo amigo
        
        }       
    }
}