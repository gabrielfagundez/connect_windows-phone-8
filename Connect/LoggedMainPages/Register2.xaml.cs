using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Connect.Resources;
using System.IO;
using System.Windows.Threading;
using Newtonsoft.Json;
using Connect.Classes;
using Connect;
using Facebook;
using System.Xml.Serialization;
using System.Text;
using Hammock.Web;
using Hammock;
using Hammock.Authentication.OAuth;
using SampleLinkedApp;
using SampleLinkedApp.Data;


namespace Connect.LoggedMainPages
{
    public partial class Register2 : PhoneApplicationPage
    {

        string OAuthTokenKey = string.Empty;
        string tokenSecret = string.Empty;
        string accessToken = string.Empty;
        string accessTokenSecret = string.Empty;

        XmlSerializer personXerializer;
        person currentPerson;

        
        public Register2()
        {
            InitializeComponent();
            btnFacebookLogin.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnFacebookLogin_Click(object sender, RoutedEventArgs e)
        {
            btnFacebookLogin.Visibility = System.Windows.Visibility.Collapsed;
            NavigationService.Navigate(new Uri("/LoggedMainPages/FacebookLoginPage.xaml", UriKind.Relative));
        }





        private void Click_check(object sender, EventArgs e)
        {
            // NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
            try
            {
                var webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                webClient.UploadStringCompleted += this.sendPostCompleted;
                LoggedUser user = LoggedUser.Instance;
                UserData u = user.GetLoggedUser();
                string json = "{\"Name\":\"" + u.Name + "\"," +
                                "\"Email\":\"" + u.Email + "\"," +
                                 "\"FacebookId\":\"" + u.FacebookId + "\"," +
                                  "\"LinkedInId\":\"" + u.LinkedInId + "\"," +
                                  "\"Password\":\"" + u.Password + "\"}";
                System.Diagnostics.Debug.WriteLine(json);

                webClient.UploadStringAsync((new Uri("http://servidorpis.azurewebsites.net/api/SignUp/")), "POST", json);
            }
            catch (WebException webex)
            {
                HttpWebResponse webResp = (HttpWebResponse)webex.Response;

                switch (webResp.StatusCode)
                {
                    case HttpStatusCode.NotFound: // 404
                        break;
                    case HttpStatusCode.InternalServerError: // 500
                        break;
                    default:
                        break;
                }
            }
        }

        private void sendPostCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if ((e.Error != null) && (e.Error.GetType().Name == "WebException"))
            {
                WebException we = (WebException)e.Error;
                HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                switch (response.StatusCode)
                {

                    case HttpStatusCode.NotFound: // 404
                        System.Diagnostics.Debug.WriteLine("Not found!");
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");
                        break;
                    default:
                        break;
                }
            }
            else
            {                
                LoggedUser user = LoggedUser.Instance;
                user.SetLoggedUser(JsonConvert.DeserializeObject<UserData>(e.Result));
                MainUtil.SetKeyValue<string>("AccessToken", string.Empty);
                MainUtil.SetKeyValue<string>("AccessTokenSecret", string.Empty);
                NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
            }
        }





        private void MenuItemSignIn_Click(object sender, EventArgs e)
        {
            accessToken = MainUtil.GetKeyValue<string>("AccessToken");
            accessTokenSecret = MainUtil.GetKeyValue<string>("AccessTokenSecret");

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(accessTokenSecret))
            {
                var requestTokenQuery = OAuthUtil.GetRequestTokenQuery();
                requestTokenQuery.RequestAsync(AppSettings.RequestTokenUri, null);
                requestTokenQuery.QueryResponse += new EventHandler<WebQueryResponseEventArgs>(requestTokenQuery_QueryResponse);
            }
            else
            {
                Dispatcher.BeginInvoke(() =>
                {
                    // finding current user's profile if already signed in
                    getCurrentUserProfile();                   

                });
            }
        }

        void requestTokenQuery_QueryResponse(object sender, WebQueryResponseEventArgs e)
        {
            try
            {
                StreamReader reader = new StreamReader(e.Response);
                string strResponse = reader.ReadToEnd();
                var parameters = MainUtil.GetQueryParameters(strResponse);
                OAuthTokenKey = parameters["oauth_token"];
                tokenSecret = parameters["oauth_token_secret"];
                var authorizeUrl = AppSettings.AuthorizeUri + "?oauth_token=" + OAuthTokenKey;

                Dispatcher.BeginInvoke(() =>
                {
                    this.loginBrowserControl.Navigate(new Uri(authorizeUrl, UriKind.RelativeOrAbsolute));
                });
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(ex.Message);
                });
            }
        }

        private void loginBrowserControl_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.loginBrowserControl.Visibility = Visibility.Visible;            
            this.loginBrowserControl.Navigated -= loginBrowserControl_Navigated;
        }

        private void loginBrowserControl_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.ToString().StartsWith(AppSettings.CallbackUri) && e.Uri.ToString().Contains("oauth_verifier"))
            {
                var AuthorizeResult = MainUtil.GetQueryParameters(e.Uri.ToString());
                var VerifyPin = AuthorizeResult["oauth_verifier"];
                this.loginBrowserControl.Visibility = Visibility.Collapsed;

                getAccessToken(VerifyPin);
            }
        }

        private void getAccessToken(String aVerifyPin)
        {
            var credentials = new OAuthCredentials
            {
                Type = OAuthType.AccessToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = AppSettings.consumerKey,
                ConsumerSecret = AppSettings.consumerKeySecret,
                Token = this.OAuthTokenKey,
                TokenSecret = this.tokenSecret,
                Verifier = aVerifyPin,
                Version = "1.0"
            };

            var client = new RestClient
            {
                Authority = "https://api.linkedin.com/uas/oauth",
                HasElevatedPermissions = true
            };

            var request = new RestRequest
            {
                Credentials = credentials,
                Path = "/accessToken",
                Method = WebMethod.Post
            };

            client.BeginRequest(request, new RestCallback(AccessTokenRequestCallback));
        }

        private void AccessTokenRequestCallback(RestRequest request, RestResponse response, object obj)
        {
            try
            {
                string respContent = response.Content;
                var parameters = MainUtil.GetQueryParameters(respContent);
                accessToken = parameters["oauth_token"];
                accessTokenSecret = parameters["oauth_token_secret"];

                MainUtil.SetKeyValue<string>("AccessToken", accessToken);
                MainUtil.SetKeyValue<string>("AccessTokenSecret", accessTokenSecret);

                Dispatcher.BeginInvoke(() =>
                {
                    // finding current user's profile 
                    getCurrentUserProfile();

                });
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(ex.Message);
                });
            }
        }

        private void getCurrentUserProfile()
        {
            var credentials = new OAuthCredentials
            {
                Type = OAuthType.ProtectedResource,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.UrlOrPostParameters,
                ConsumerKey = AppSettings.consumerKey,
                ConsumerSecret = AppSettings.consumerKeySecret,
                Token = this.accessToken,
                TokenSecret = this.accessTokenSecret,
                Version = "1.0"
            };

            var client = new RestClient
            {
                Authority = "https://api.linkedin.com",
                HasElevatedPermissions = true
            };

            var request = new RestRequest
            {
                Credentials = credentials,
                Path = "/v1/people/~",
                Method = WebMethod.Get
            };

            client.BeginRequest(request, new RestCallback(CurrentUserProfileRequestCallback));
        }

        private void CurrentUserProfileRequestCallback(RestRequest request, RestResponse response, object obj)
        {
            try
            {
                string respContent = response.Content;

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    // updating UI              
                    parseCurrentPersonData(respContent);
                });
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(ex.Message);
                });
            }
        }

        private void parseCurrentPersonData(string aRespString)
        {
            // converting string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(aRespString);
            MemoryStream personXml = new MemoryStream(byteArray);

            if (personXml != null)
            {
                personXerializer = new XmlSerializer(typeof(person));
                currentPerson = (person)personXerializer.Deserialize(personXml);
                updateUI();
            }
        }

        private void updateUI()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (currentPerson != null)
                {
                    LoggedUser user = LoggedUser.Instance;
                    UserData u = user.GetLoggedUser();
                    u.LinkedInId = currentPerson.FirstName + currentPerson.LastName;
                    user.SetLoggedUser(u);
                }
            });
        }  
    }
}