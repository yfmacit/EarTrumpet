﻿using EarTrumpet.Extensions;
using EarTrumpet.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EarTrumpet
{
    public partial class VolumeControlChannel : UserControl
    {
        public bool IsMuted { get { return (bool)GetValue(IsMutedProperty); } set { SetValue(IsMutedProperty, value); } }
        public static readonly DependencyProperty IsMutedProperty =
            DependencyProperty.Register("IsMuted", typeof(bool), typeof(VolumeControlChannel), new PropertyMetadata(false));

        public ImageSource IconSource { get { return (ImageSource)GetValue(IconUriProperty); } set { SetValue(IconUriProperty, value); } }
        public static readonly DependencyProperty IconUriProperty =
            DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(VolumeControlChannel), new PropertyMetadata(null));

        public int Volume { get { return (int)GetValue(VolumeProperty); } set { SetValue(VolumeProperty, value); } }
        public static readonly DependencyProperty VolumeProperty =
            DependencyProperty.Register("Volume", typeof(int), typeof(VolumeControlChannel), new PropertyMetadata(0));

        public string DisplayName { get { return (string)GetValue(DisplayNameProperty); } set { SetValue(DisplayNameProperty, value); } }
        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register("DisplayName", typeof(string), typeof(VolumeControlChannel), new PropertyMetadata(""));

        public int IconWidth { get { return (int)GetValue(IconWidthProperty); } set { SetValue(IconWidthProperty, value); } }
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth", typeof(int), typeof(VolumeControlChannel), new PropertyMetadata(0));

        public int IconHeight { get { return (int)GetValue(IconHeightProperty); } set { SetValue(IconHeightProperty, value); } }
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register("IconHeight", typeof(int), typeof(VolumeControlChannel), new PropertyMetadata(0));

        public FontWeight IconTextFontWeight { get { return (FontWeight)GetValue(IconTextFontWeightProperty); } set { SetValue(IconTextFontWeightProperty, value); } }
        public static readonly DependencyProperty IconTextFontWeightProperty =
            DependencyProperty.Register("IconTextFontWeight", typeof(FontWeight), typeof(VolumeControlChannel), new PropertyMetadata(FontWeights.Normal));

        public string IconText { get { return (string)GetValue(IconTextProperty); } set { SetValue(IconTextProperty, value); } }
        public static readonly DependencyProperty IconTextProperty =
            DependencyProperty.Register("IconText", typeof(string), typeof(VolumeControlChannel), new PropertyMetadata(""));

        public float PeakValue { get { return (float)GetValue(PeakValueProperty); } set { SetValue(PeakValueProperty, value); } }
        public static readonly DependencyProperty PeakValueProperty =
            DependencyProperty.Register("PeakValue", typeof(float), typeof(VolumeControlChannel), new PropertyMetadata(0f));

        public bool BeepOnPointerUp { get { return (bool)GetValue(BeepOnPointerUpProperty); } set { SetValue(BeepOnPointerUpProperty, value); } }
        public static readonly DependencyProperty BeepOnPointerUpProperty =
            DependencyProperty.Register("BeepOnPointerUp", typeof(bool), typeof(VolumeControlChannel), new PropertyMetadata(false));

        public FontFamily IconTextFontFamily { get { return (FontFamily)GetValue(IconTextFontFamilyProperty); } set { SetValue(IconTextFontFamilyProperty, value); } }
        public static readonly DependencyProperty IconTextFontFamilyProperty =
            DependencyProperty.Register("IconTextFontFamily", typeof(FontFamily), typeof(VolumeControlChannel), new PropertyMetadata(null));

        public int IconTextSize { get { return (int)GetValue(IconTextSizeProperty); } set { SetValue(IconTextSizeProperty, value); } }
        public static readonly DependencyProperty IconTextSizeProperty =
            DependencyProperty.Register("IconTextSize", typeof(int), typeof(VolumeControlChannel), new PropertyMetadata(0));

        public int VolumeTextFontSize { get { return (int)GetValue(VolumeTextFontSizeProperty); } set { SetValue(VolumeTextFontSizeProperty, value); } }
        public static readonly DependencyProperty VolumeTextFontSizeProperty =
            DependencyProperty.Register("VolumeTextFontSize", typeof(int), typeof(VolumeControlChannel), new PropertyMetadata(0));

        public FontWeight VolumeTextFontWeight { get { return (FontWeight)GetValue(fontWeightProperty); } set { SetValue(fontWeightProperty, value); } }
        public static readonly DependencyProperty fontWeightProperty =
            DependencyProperty.Register("VolumeTextFontWeight", typeof(FontWeight), typeof(VolumeControlChannel), new PropertyMetadata(FontWeights.Normal));

        public VolumeControlChannel()
        {
            InitializeComponent();
            GridRoot.DataContext = this;

            ThemeService.UpdateThemeResources(Resources);
        }

        private void Slider_TouchDown(object sender, TouchEventArgs e)
        {
            VisualStateManager.GoToState((FrameworkElement)sender, "Pressed", true);

            var slider = (Slider)sender;
            slider.SetPositionByControlPoint(e.GetTouchPoint(slider).Position);
            slider.CaptureTouch(e.TouchDevice);

            IsMuted = false;

            e.Handled = true;
        }

        private void Slider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                VisualStateManager.GoToState((FrameworkElement)sender, "Pressed", true);

                var slider = (Slider)sender;
                slider.SetPositionByControlPoint(e.GetPosition(slider));
                slider.CaptureMouse();

                IsMuted = false;

                e.Handled = true;
            }
        }

        private void Slider_TouchUp(object sender, TouchEventArgs e)
        {
            VisualStateManager.GoToState((FrameworkElement)sender, "Normal", true);

            var slider = (Slider)sender;
            slider.ReleaseTouchCapture(e.TouchDevice);
            e.Handled = true;

            if (BeepOnPointerUp) System.Media.SystemSounds.Beep.Play();
        }

        private void Slider_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var slider = (Slider)sender;
            if (slider.IsMouseCaptured)
            {
                // If the point is outside of the control, clear the hover state.
                Rect rcSlider = new Rect(0, 0, slider.ActualWidth, slider.ActualHeight);
                if (!rcSlider.Contains(e.GetPosition(slider)))
                {
                    VisualStateManager.GoToState((FrameworkElement)sender, "Normal", true);
                }

                ((Slider)sender).ReleaseMouseCapture();
                e.Handled = true;
            }

            if (BeepOnPointerUp) System.Media.SystemSounds.Beep.Play();
        }

        private void Slider_TouchMove(object sender, TouchEventArgs e)
        {
            var slider = (Slider)sender;
            if (slider.AreAnyTouchesCaptured)
            {
                slider.SetPositionByControlPoint(e.GetTouchPoint(slider).Position);
                e.Handled = true;
            }
        }

        private void Slider_MouseMove(object sender, MouseEventArgs e)
        {
            var slider = (Slider)sender;
            if (slider.IsMouseCaptured)
            {
                slider.SetPositionByControlPoint(e.GetPosition(slider));
            }
        }

        private void Slider_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var slider = (Slider)sender;
            var amount = Math.Sign(e.Delta) * 2.0;
            slider.ChangePositionByAmount(amount);
            e.Handled = true;
        }

        private void Mute_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsMuted = !IsMuted;
                e.Handled = true;
            }
        }

        private void Icon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsMuted = !IsMuted;
                e.Handled = true;
            }
        }
    }
}
