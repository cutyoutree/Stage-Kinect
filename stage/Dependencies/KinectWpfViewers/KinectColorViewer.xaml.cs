namespace Microsoft.Samples.Kinect.WpfViewers
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;

    /// <summary>
    /// Interaction logic for KinectColorViewer.xaml
    /// </summary>
    public partial class KinectColorViewer : ImageViewer
    {
        private static readonly int Bgr32BytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        private ColorImageFormat lastImageFormat = ColorImageFormat.Undefined;
        private byte[] pixelData;
        private WriteableBitmap outputImage;

        public KinectColorViewer()
        {
            InitializeComponent();
        }

        protected override void OnKinectChanged(KinectSensor oldKinectSensor, KinectSensor newKinectSensor)
        {
            if (oldKinectSensor != null)
            {
                oldKinectSensor.ColorFrameReady -= this.ColorImageReady;
                kinectColorImage.Source = null;
                this.lastImageFormat = ColorImageFormat.Undefined;
            }

            if (newKinectSensor != null && newKinectSensor.Status == KinectStatus.Connected)
            {
                ResetFrameRateCounters();

                if (newKinectSensor.ColorStream.Format == ColorImageFormat.RawYuvResolution640x480Fps15)
                {
                    throw new NotImplementedException("RawYuv conversion is not yet implemented.");
                }
                else
                {
                    newKinectSensor.ColorFrameReady += this.ColorImageReady;
                }
            }
        }

        private void ColorImageReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame imageFrame = e.OpenColorImageFrame())
            {
                if (imageFrame != null)
                {
                    // We need to detect if the format has changed.
                    bool haveNewFormat = this.lastImageFormat != imageFrame.Format;

                    if (haveNewFormat)
                    {
                        this.pixelData = new byte[imageFrame.PixelDataLength];
                    }

                    imageFrame.CopyPixelDataTo(this.pixelData);

                    // A WriteableBitmap is a WPF construct that enables resetting the Bits of the image.
                    // This is more efficient than creating a new Bitmap every frame.
                    if (haveNewFormat)
                    {
                        kinectColorImage.Visibility = Visibility.Visible;
                        this.outputImage = new WriteableBitmap(
                            imageFrame.Width, 
                            imageFrame.Height,
                            96,  // DpiX
                            96,  // DpiY
                            PixelFormats.Bgr32, 
                            null);

                        this.kinectColorImage.Source = this.outputImage;
                    }

                    this.outputImage.WritePixels(
                        new Int32Rect(0, 0, imageFrame.Width, imageFrame.Height),
                        this.pixelData,
                        imageFrame.Width * Bgr32BytesPerPixel,
                        0);

                    this.lastImageFormat = imageFrame.Format;

                    UpdateFrameRate();
                }
            }
        }
    }
}
