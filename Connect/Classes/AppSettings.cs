using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Connect.Classes
{
    public class AppSettings
    {

        // linked in
        public static string RequestTokenUri = "https://api.linkedin.com/uas/oauth/requestToken";
        public static string AuthorizeUri = "https://www.linkedin.com/uas/oauth/authorize";
        public static string AccessTokenUri = "https://api.linkedin.com/uas/oauth/accessToken";     
 
        public static string CallbackUri = "http://localhost";
        public static string consumerKey = "ou9xvx3lni8k";
        public static string consumerKeySecret = "TkhemWQTIziYMAPZ";

        public static string oAuthVersion = "1.0";

        //general Strings
        public static string TEXT_WELCOME = "Welcome";
        public static string SIGNED_OUT_SUCCESSFULLY = "You have been signed out successfully."; 
    }
}
