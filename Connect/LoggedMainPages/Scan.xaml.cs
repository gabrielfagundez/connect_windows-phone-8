using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices;
using System.Threading;
using System.Windows.Media.Imaging;
using ZXing;
using System.Windows.Threading;
using ZXing.QrCode;
using Connect.Classes;
using Newtonsoft.Json;
using Connect;
using System.Windows.Media;
using Connect.Resources;
using Microsoft.Phone.Net.NetworkInformation;

namespace ZXLib_Test_WP7
{
    public partial class Scan : PhoneApplicationPage
    {
        private PhotoCamera _phoneCamera;
        private IBarcodeReader _barcodeReader;
        private DispatcherTimer _scanTimer;
        private WriteableBitmap _previewBuffer;


        DispatcherTimer newTimer = new DispatcherTimer();//sacar, solo para probar sin cel!!!!!!!!!!!!!!!!!!!!!!!

        public Scan()
        {
            InitializeComponent();            
        }

        
        void OnTimerTick(Object sender, EventArgs args)//sacar, solo para probar sin cel
        {
            qrFound("55");
            newTimer.Stop();

        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            Connecting.Text = AppResources.searching;
            ProgressB.IsIndeterminate = false;
            Connecting.Visibility = System.Windows.Visibility.Visible;

            //***************************sacar, solo para probar sin cel*************************************
            // timer interval specified as 1 second
            newTimer.Interval = TimeSpan.FromSeconds(2);
            // Sub-routine OnTimerTick will be called at every 1 second
            newTimer.Tick += OnTimerTick;
            // starting the timer
            newTimer.Start();
            //*************************************************************************************



            // Initialize the camera object
            _phoneCamera = new PhotoCamera();
            _phoneCamera.Initialized += cam_Initialized;

            CameraButtons.ShutterKeyHalfPressed += CameraButtons_ShutterKeyHalfPressed;

            //Display the camera feed in the UI
            viewfinderBrush.SetSource(_phoneCamera);


            // This timer will be used to scan the camera buffer every 250ms and scan for any barcodes
            _scanTimer = new DispatcherTimer();
            _scanTimer.Interval = TimeSpan.FromMilliseconds(250);
            _scanTimer.Tick += (o, arg) => ScanForBarcode();

            base.OnNavigatedTo(e);
        }

        void CameraButtons_ShutterKeyHalfPressed(object sender, EventArgs e)
        {
            _phoneCamera.Focus();
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            //we're navigating away from this page, we won't be scanning any barcodes
            _scanTimer.Stop();

            if (_phoneCamera != null)
            {
                // Cleanup
                _phoneCamera.Dispose();
                _phoneCamera.Initialized -= cam_Initialized;
                CameraButtons.ShutterKeyHalfPressed -= CameraButtons_ShutterKeyHalfPressed;
            }
        }

        void cam_Initialized(object sender, Microsoft.Devices.CameraOperationCompletedEventArgs e)
        {
            if (e.Succeeded)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    _phoneCamera.FlashMode = FlashMode.Off;
                    _previewBuffer = new WriteableBitmap((int)_phoneCamera.PreviewResolution.Width, (int)_phoneCamera.PreviewResolution.Height);

                    _barcodeReader = new BarcodeReader();

                    // By default, BarcodeReader will scan every supported barcode type
                    // If we want to limit the type of barcodes our app can read, 
                    // we can do it by adding each format to this list object

                    //var supportedBarcodeFormats = new List<BarcodeFormat>();
                    //supportedBarcodeFormats.Add(BarcodeFormat.QR_CODE);
                    //supportedBarcodeFormats.Add(BarcodeFormat.DATA_MATRIX);
                    //_bcReader.PossibleFormats = supportedBarcodeFormats;

                    _barcodeReader.TryHarder = true;

                    _barcodeReader.ResultFound += _bcReader_ResultFound;
                    _scanTimer.Start();
                });
            }
            else
            {
                Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show(AppResources.CameraProblem);
                    });
            }
        }

        void _bcReader_ResultFound(Result obj)
        {
            qrFound(obj.Text);
        }

        private void ScanForBarcode()
        {
            //grab a camera snapshot
            _phoneCamera.GetPreviewBufferArgb32(_previewBuffer.Pixels);
            _previewBuffer.Invalidate();

            //scan the captured snapshot for barcodes
            //if a barcode is found, the ResultFound event will fire
            _barcodeReader.Decode(_previewBuffer);

        }
        private bool IsNetworkAvailable()
        {
            if (App.isDebug)
                return false;
            else if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
                return false;
            else
                return true;
        }

        private void apagarCam()
        {
            //apaga cam y scan******************************************
            _scanTimer.Stop();
            if (_phoneCamera != null)
            {
                // Cleanup
                _phoneCamera.Dispose();
                _phoneCamera.Initialized -= cam_Initialized;
                CameraButtons.ShutterKeyHalfPressed -= CameraButtons_ShutterKeyHalfPressed;
            }
            //*********************************************************************
        }


        private void qrFound(string idQR)
        {
            VibrateController.Default.Start(TimeSpan.FromMilliseconds(100));
            apagarCam();
            ProgressB.IsIndeterminate = true;
            Connecting.Text = AppResources.scanning;
            Connecting.Visibility = System.Windows.Visibility.Visible;
            LoggedUser user = LoggedUser.Instance;
            UserData _userData = user.GetLoggedUser();
            if (idQR == _userData.Id)
            {
                errorFunc(AppResources.sameID);
            }
            else
            {
                getInfo2(idQR);
            }
        }
        private void getInfo2(string id)
        {
            {
                //NavigationService.Navigate(new Uri("/LoggedMainPages/Scan.xaml", UriKind.Relative));
                if (IsNetworkAvailable())
                {
                    try
                    {


                        var webClient = new WebClient();
                        webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                        webClient.UploadStringCompleted += this.sendPostCompleted;
                        LoggedUser user = LoggedUser.Instance;
                        UserData _userData = user.GetLoggedUser();
                        string json = "{\"IdFrom\":\"" + _userData.Id + "\"," +
                                            "\"IdTo\":\"" + id + "\"}";

                        webClient.UploadStringAsync((new Uri(App.webService + "/api/Friends/AddFriendFromIds")), "POST", json);

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
                else
                {
                    SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
                    CustomMessageBox messageBox = new CustomMessageBox()
                    {
                        Caption = AppResources.NoInternetConnection,
                        Message = AppResources.NoInternetConnectionMessage,
                        LeftButtonContent = AppResources.OkTitle,
                        Background = mybrush,
                        IsFullScreen = false,
                    };


                    messageBox.Dismissed += (s1, e1) =>
                    {
                        switch (e1.Result)
                        {
                            case CustomMessageBoxResult.LeftButton:
                                apagarCam();
                                NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml.xaml", UriKind.Relative));
                                break;
                            case CustomMessageBoxResult.None:
                                // Acción.
                                break;
                            default:
                                break;
                        }
                    };

                    messageBox.Show();


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
                        errorFunc(AppResources.notFound);
                        ProgressB.IsIndeterminate = false;
                        Connecting.Visibility = System.Windows.Visibility.Collapsed;

                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        errorFunc(AppResources.notAuth);
                        ProgressB.IsIndeterminate = false;
                        Connecting.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                UserData usuarioRecibido = JsonConvert.DeserializeObject<UserData>(e.Result);
                ProgressB.IsIndeterminate = false;
                Connecting.Visibility = System.Windows.Visibility.Collapsed;
                LoggedUser lu = LoggedUser.Instance;

                lu.friendInf = new UserData();
                lu.friendInf.Name = usuarioRecibido.Name;
                lu.friendInf.Mail = usuarioRecibido.Mail;

                lu.friendInf.FacebookId = usuarioRecibido.FacebookId;
                lu.friendInf.LinkedInId = usuarioRecibido.LinkedInId;

                if (usuarioRecibido.FacebookId == "not connected" || usuarioRecibido.FacebookId == null)
                {
                    lu.friendInf.FacebookId = "";
                }

                if (usuarioRecibido.LinkedInId == "not connected" || usuarioRecibido.FacebookId == null)
                {
                    lu.friendInf.LinkedInId = "";
                }
                System.Diagnostics.Debug.WriteLine("Finish scann:" + DateTime.Now.Second.ToString());
                NavigationService.Navigate(new Uri("/LoggedMainPages/FriendInfo.xaml", UriKind.Relative));
            }
        }



        private void errorFunc(string text)
        {
            SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "",
                Message = text,
                LeftButtonContent = AppResources.OkTitle,

                Background = mybrush,
                IsFullScreen = false
            };
            messageBox.Show();
            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        NavigationService.Navigate(new Uri("/LoggedMainPages/LoggedMainPage.xaml", UriKind.Relative));
                        break;
                    case CustomMessageBoxResult.RightButton:
                        // Acción.
                        break;
                    case CustomMessageBoxResult.None:
                        // Acción.
                        break;
                    default:
                        break;
                }
            };
            
        }

    }
}