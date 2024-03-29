namespace Microsoft.Samples.Kinect.WpfViewers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Controls;
    using Microsoft.Kinect;

    /// <summary>
    /// Interaction logic for KinectDiagnosticViewer.xaml
    /// </summary>
    public partial class KinectDiagnosticViewer : UserControl
    {
        private readonly KinectSettings kinectSettings;
        private readonly Dictionary<KinectSensor, bool> sensorIsInitialized = new Dictionary<KinectSensor, bool>();
        private KinectSensor kinect;
        private bool kinectAppConflict;

        public KinectDiagnosticViewer()
        {
            InitializeComponent();
            this.kinectSettings = new KinectSettings(this);
            this.kinectSettings.PopulateComboBoxesWithFormatChoices();
            Settings.Content = this.kinectSettings;
            this.KinectColorViewer = this.colorViewer;
            this.StatusChanged();
        }

        public ImageViewer KinectColorViewer { get; set; }

        public KinectSensor Kinect
        {
            get
            {
                return this.kinect;
            }

            set
            {
                if (this.kinect != null)
                {
                    bool wasInitialized;
                    this.sensorIsInitialized.TryGetValue(this.kinect, out wasInitialized);
                    if (wasInitialized)
                    {
                        this.UninitializeKinectServices(this.kinect);
                        this.sensorIsInitialized[this.kinect] = false;
                    }
                }

                this.kinect = value;
                this.kinectSettings.Kinect = value;
                if (this.kinect != null)
                {
                    if (this.kinect.Status == KinectStatus.Connected)
                    {
                        this.kinect = this.InitializeKinectServices(this.kinect);

                        if (this.kinect != null)
                        {
                            this.sensorIsInitialized[this.kinect] = true;
                        }
                    }
                }

                this.StatusChanged(); // update the UI about this sensor
            }
        }

        public void StatusChanged()
        {
            if (this.kinectAppConflict)
            {
                status.Text = "KinectAppConflict";
            }
            else if (this.Kinect == null)
            {
                status.Text = "Kinect initialize failed";
            }
            else
            {
                this.status.Text = this.Kinect.Status.ToString();

                if (this.Kinect.Status == KinectStatus.Connected)
                {
                    // Update comboboxes' selected value based on stream isenabled/format.
                    this.kinectSettings.colorFormats.SelectedValue = this.Kinect.ColorStream.Format;
                    this.kinectSettings.depthFormats.SelectedValue = this.Kinect.DepthStream.Format;
                    this.kinectSettings.trackingModes.SelectedValue = KinectSkeletonViewerOnDepth.TrackingMode;
                    
                    this.kinectSettings.UpdateUiElevationAngleFromSensor();
                }
            }
        }
        
        // Kinect enabled apps should customize which Kinect services it initializes here.
        private KinectSensor InitializeKinectServices(KinectSensor sensor)
        {
            // Centralized control of the formats for Color/Depth and enabling skeletalViewer
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            sensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
            this.kinectSettings.SkeletonStreamEnable.IsChecked = true; // will enable SkeletonStream if available

            // Inform the viewers of the Kinect KinectSensor.
            this.KinectColorViewer.Kinect = sensor;
            KinectDepthViewer.Kinect = sensor;
            KinectSkeletonViewerOnColor.Kinect = sensor;
            KinectSkeletonViewerOnDepth.Kinect = sensor;
            kinectAudioViewer.Kinect = sensor;

            // Start streaming
            try
            {
                sensor.Start();
                this.kinectAppConflict = false;
            }
            catch (IOException)
            {
                this.kinectAppConflict = true;
                return null;
            }

            sensor.AudioSource.Start();
            return sensor;
        }

        // Kinect enabled apps should uninitialize all Kinect services that were initialized in InitializeKinectServices() here.
        private void UninitializeKinectServices(KinectSensor sensor)
        {
            sensor.AudioSource.Stop();

            // Stop streaming
            sensor.Stop();

            // Inform the viewers that they no longer have a Kinect KinectSensor.
            this.KinectColorViewer.Kinect = null;
            KinectDepthViewer.Kinect = null;
            KinectSkeletonViewerOnColor.Kinect = null;
            KinectSkeletonViewerOnDepth.Kinect = null;
            kinectAudioViewer.Kinect = null;

            // Disable skeletonengine, as only one Kinect can have it enabled at a time.
            if (sensor.SkeletonStream != null)
            {
                sensor.SkeletonStream.Disable();
            }
        }
    }
}
