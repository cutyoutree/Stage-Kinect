   M S B A M L   `   `   `  ����^  ZMicrosoft.Samples.Kinect.WpfViewers, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null�^clr-namespace:Microsoft.Samples.Kinect.WpfViewers;assembly=Microsoft.Samples.Kinect.WpfViewers#Microsoft.Samples.Kinect.WpfViewers  5   	    SkeletalTracking!   SkeletalTracking.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   mmy^clr-namespace:Microsoft.Samples.Kinect.WpfViewers;assembly=Microsoft.Samples.Kinect.WpfViewers     5   	   -   5          � Title$"  SkeletonTracker Metin Yilmaz��5   	   $	��600��6.   $	��800��6;    � WindowState$ 	Maximized=�5   "    � 
ResizeMode$ 	CanResize=�6:    � SizeToContent$ Manual=�6S    � Icon$8 2/SkeletalTracking;component/Images/Kinect_Icon.ico��6j   .��5      �� -   5        $��
MainCanvas��5      .+�5   
   ;   5Microsoft.Samples.Kinect.WpfViewers.KinectColorViewer  -   5        $��kinectColorViewer1��5   Y    �� Left$ -3��6   
 �� Top$ -6��60   $	��640��6@   $	��480��6L   5   /Microsoft.Samples.Kinect.WpfViewers.ImageViewer   Kinect 5	      ��  �� ElementName$ kinectSensorChooser1��	 �� Path$	 Kinect�A� 5   
   -   5        $��leftEllipse��5   =   $ 0��6   $ 0��6"   $��50��61   $��50��6Q   
��� ��
 J� Opacity$
 1Q�6n   
���   �6z   A� 5   
   -   5        $��rightEllipse��5   K   $ 57��6   $ 0��6#   
���   �$��50��6?   $��50��6_   $
 1Q�6j   
��� �� � DataContext 6�   �� A� 5   
   -   5        $��rightShoulderEllipse��5   >   $ 0��6   $ 12��6"   $��50��62   $��50��6[   
���  ��$
 1Q�6q   
���   �6}   A� 5   
   -   5        $��leftShoulderEllipse��5   J   $ 57��6   $ 12��6#   
���  ��$��50��6>   $��50��6e   $
 1Q�6p   
���   � 6�   �� A� 5   
   -   5        $��centerShoulderEllipse��5   >   $ 0��6   $ 23��6"   $��50��62   $��50��6\   
����  �$
 1Q�6t   
���   �6�   A� 5   
   -	   5        $��spineEllipse��5   ?   $ 57��6   $ 23��6#   $��50��63   $��50��6T   
���2͚�$
 1Q�6s   
���   �6   A� 5   
   -
   5        $��centerHipEllipse��5   @   $	 113��6   $ 23��6$   $��50��64   $��50��6Y   
���  ��$
 1Q�6t   
���   �6�   =   7Microsoft.Samples.Kinect.WpfViewers.KinectSensorChooser  5   
   -   5        $��kinectSensorChooser1��5   C   $ 12��6!   $	 615��62   $��0��6_   $��2��6i   �� 5   
   -   5        $��	headImage��5   =   $ 0��6   $ 79��6    $	��120��60   $
��Fill=�6N   $	��120��6]   $/��)/SkeletalTracking;component/c4f-color.png��6i   >   8Microsoft.Samples.Kinect.WpfViewers.KinectSkeletonViewer  5   
   -   5        $��kinectSkeletonViewer2��5   E   $	 817��6"   $	 -24��64   $	��240��6b   $	��320��6o    6{   �� $ kinectSensorChooser1��$	 Kinect�;   5Microsoft.Samples.Kinect.WpfViewers.KinectDepthViewer  5   
   -   5        $��kinectDepthViewer1��5   B   $	 817��6   $	 234��61   $	��240��6\   $	��320��6i    6u   �� $ kinectSensorChooser1��$	 Kinect�  5   
   $	��100��6   $	 281��6,   $	 165��6>   $	��100��6O   5      5      