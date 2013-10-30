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
            this.loginBrowserControl.Navigate(new Uri("http://www.linkedin.com/profile/view?id=" + lu.friendInf.LinkedInId + "&authType=NAME_SEARCH&authToken=POOH&locale=en_US&srchid=2875693011383147764200&srchindex=1&srchtotal=1&trk=vsrp_people_res_name&trkInfo=VSRPsearchId%3A2875693011383147764200%2CVSRPtargetId%3A709226%2CVSRPcmpt%3Aprimary", UriKind.Absolute));//aca va el linkedin del nuevo amigo
        }       
    }
}