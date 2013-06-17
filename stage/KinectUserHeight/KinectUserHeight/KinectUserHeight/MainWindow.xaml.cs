using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace KinectUserHeight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor _sensor;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.KinectSensors.Where(x => x.Status == KinectStatus.Connected).FirstOrDefault();

            if (_sensor != null)
            {
                _sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(Sensor_SkeletonFrameReady);
                _sensor.SkeletonStream.Enable();

                _sensor.Start();
            }
        }

        void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    canvas.Children.Clear();
                    //Maakt een Skelet aan
                    Skeleton[] skeletons = new Skeleton[frame.SkeletonArrayLength];
                    
                    frame.CopySkeletonDataTo(skeletons);
                    //Track het eerste skelet die hij ziet
                    var skeleton = skeletons.Where(s => s.TrackingState == SkeletonTrackingState.Tracked).FirstOrDefault();
                   
                    if (skeleton != null)
                    {
                        // Berekent de hoogte
                        double height = Math.Round(skeleton.Height(), 2);

                        // Tekent de Skeleton joints.
                        foreach (JointType joint in Enum.GetValues(typeof(JointType)))
                        {
                            DrawJoint(skeleton.Joints[joint].ScaleTo(640, 480));
                        }

                        // Print de hoogte.
                        tblHeight.Text = "Height: " + height.ToString() + "m";
                    }
                }
            }
        }

        private void DrawJoint(Joint joint)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = new SolidColorBrush(Colors.LightCoral)
            };

            Canvas.SetLeft(ellipse, joint.Position.X);
            Canvas.SetTop(ellipse, joint.Position.Y);

            canvas.Children.Add(ellipse);
        }
    }
}
