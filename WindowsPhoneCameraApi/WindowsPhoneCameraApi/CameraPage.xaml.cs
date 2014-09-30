using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace WindowsPhoneCameraApi
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CameraPage : Page
    {
        public MediaCapture takePhotoManager { get; set; }
        public CameraPage()
        {
            this.InitializeComponent();

            takePhotoManager = new MediaCapture();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Initialize the camera.
            try
            {
                InitCamera();
            }
            catch (Exception ex)
            {
                var test = "";
            }
        }

        //Initializing uses async methods, therefore the InitCamera method must itself be async.
        private async void InitCamera()
        {
            try
            {
                //Get the camera ID.
                var cameraID = await GetCameraID(Windows.Devices.Enumeration.Panel.Back);

                //Initialize the Capture Manager with settings.
                await takePhotoManager.InitializeAsync(new MediaCaptureInitializationSettings
                {
                    StreamingCaptureMode = StreamingCaptureMode.Video,
                    PhotoCaptureSource = PhotoCaptureSource.Photo,
                    AudioDeviceId = string.Empty,
                    VideoDeviceId = cameraID.Id
                });

                //Once the manager has been initialized, add it to the PhotoPreview control
                PhotoPreview.Source = takePhotoManager;

                //Then start previewing.
                StartPreview();
            }
            catch (Exception ex)
            {
                var test = "";
            }

        }

        private static async Task<DeviceInformation> GetCameraID(Windows.Devices.Enumeration.Panel desired)
        {
            DeviceInformation deviceID = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture))
                .FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == desired);

            if (deviceID != null) return deviceID;
            else throw new Exception(string.Format("Camera of type {0} doesn't exist.", desired));
        }

        private async void StartPreview()
        {
            await takePhotoManager.StartPreviewAsync();
        }
    }
}
