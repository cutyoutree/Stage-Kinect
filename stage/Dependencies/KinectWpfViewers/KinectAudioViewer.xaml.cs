namespace Microsoft.Samples.Kinect.WpfViewers
{
    using System;
    using System.Windows.Controls;
    using Microsoft.Kinect;

    /// <summary>
    /// Interaction logic for KinectAudioViewer.xaml
    /// </summary>
    public partial class KinectAudioViewer : ImageViewer
    {
        private double angle;
        private double soundSourceAngle;

        public KinectAudioViewer()
        {
            InitializeComponent();
            this.MarkWidth = 0.05;
            this.SoundSourceWidth = 0.05;
            this.BeamAngleInDegrees = 0;
            this.BeamDisplayText = null;
            this.SoundSourceAngleInDegrees = 0;
            this.SoundSourceDisplayText = null;
        }

        /// <summary>
        /// Gets or sets string overlayed on beam indicator
        /// </summary>
        public string BeamDisplayText
        {
            get
            {
                return txtDisplayBeam.Text;
            }

            set
            {
                txtDisplayBeam.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets string overlayed on sound source indicator
        /// </summary>
        public string SoundSourceDisplayText
        {
            get
            {
                return txtDisplaySource.Text;
            }

            set
            {
                txtDisplaySource.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets width of the beam mark, in the 0-0.5 range
        /// </summary>
        public double MarkWidth { get; set; }

        /// <summary>
        /// Gets or sets audio beam angle, in degrees
        /// </summary>
        public double BeamAngleInDegrees
        {
            get
            {
                return this.angle;
            }

            set
            {
                // save RAW sensor value
                this.angle = value;

                // Angle is in Degrees, so map the MinBeamAngle..MaxBeamAngle range to 0..1
                // and clamp
                double gradientOffset = (value / (KinectAudioSource.MaxBeamAngle - KinectAudioSource.MinBeamAngle)) + 0.5;
                if (gradientOffset > 1.0)
                {
                    gradientOffset = 1.0;
                }

                if (gradientOffset < 0.0)
                {
                    gradientOffset = 0.0;
                }

                // Move the gradient stops together
                this.gsPre.Offset = Math.Max(gradientOffset - this.MarkWidth, 0);
                gsIt.Offset = gradientOffset;
                this.gsPos.Offset = Math.Min(gradientOffset + this.MarkWidth, 1);
            }
        }

        /// <summary>
        /// Gets or sets width of the sound source mark, in the 0-0.5 range
        /// </summary>
        public double SoundSourceWidth { get; set; }

        /// <summary>
        /// Gets or sets sound direction angle, in degrees
        /// </summary>
        public double SoundSourceAngleInDegrees
        {
            get
            {
                return this.soundSourceAngle;
            }

            set
            {
                // save RAW sensor value
                this.soundSourceAngle = value;

                // Angle is in Degrees, so map the MinSoundSourceAngle..MaxSoundSourceAngle range to 0..1
                // and clamp
                double gradientOffset = (value
                                         /
                                         (KinectAudioSource.MaxSoundSourceAngle - KinectAudioSource.MinSoundSourceAngle))
                                        + 0.5;
                if (gradientOffset > 1.0)
                {
                    gradientOffset = 1.0;
                }

                if (gradientOffset < 0.0)
                {
                    gradientOffset = 0.0;
                }

                // Move the gradient stops together
                this.gsPreS.Offset = Math.Max(gradientOffset - this.SoundSourceWidth, 0);
                gsItS.Offset = gradientOffset;
                this.gsPosS.Offset = Math.Min(gradientOffset + this.SoundSourceWidth, 1);
            }
        }

        protected override void OnKinectChanged(KinectSensor oldKinectSensor, KinectSensor newKinectSensor)
        {
            if (oldKinectSensor != null && oldKinectSensor.AudioSource != null)
            {
                // remove old handlers
                oldKinectSensor.AudioSource.BeamAngleChanged -= this.AudioSourceBeamChanged;
                oldKinectSensor.AudioSource.SoundSourceAngleChanged -= this.AudioSourceSoundSourceAngleChanged;
            }

            if (newKinectSensor != null && newKinectSensor.AudioSource != null)
            {
                // add new handlers
                newKinectSensor.AudioSource.BeamAngleChanged += this.AudioSourceBeamChanged;
                newKinectSensor.AudioSource.SoundSourceAngleChanged += this.AudioSourceSoundSourceAngleChanged;
            }
        }

        private void AudioSourceSoundSourceAngleChanged(object sender, SoundSourceAngleChangedEventArgs e)
        {
            // Set width of mark based on confidence
            this.SoundSourceWidth = Math.Max(((1 - e.ConfidenceLevel) / 2), 0.02);

            // Move indicator
            this.SoundSourceAngleInDegrees = e.Angle;

            // Update text
            this.SoundSourceDisplayText = " Sound source angle = " + this.SoundSourceAngleInDegrees.ToString("0.00") + " deg  Confidence level=" + e.ConfidenceLevel.ToString("0.00");
        }

        private void AudioSourceBeamChanged(object sender, BeamAngleChangedEventArgs e)
        {
            // Move our indicator
            this.BeamAngleInDegrees = e.Angle;

            // Update Text
            this.BeamDisplayText = " Audio beam angle = " + this.BeamAngleInDegrees.ToString("0.00") + " deg";
        }
    }
}
