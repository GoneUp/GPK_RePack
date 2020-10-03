using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GPK_RePack_WPF.Annotations;

namespace GPK_RePack_WPF.Controls
{
    public partial class ImagePreview : INotifyPropertyChanged
    {
        Point _start;
        Point _origin;

        public double PreviewImageWidth => Image?.Width ?? 0;
        public double PreviewImageHeight => Image?.Height ?? 0;

        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImagePreview), new PropertyMetadata(null, OnImageChanged));

        public string ImageFormat
        {
            get => (string)GetValue(ImageFormatProperty);
            set => SetValue(ImageFormatProperty, value);
        }
        public static readonly DependencyProperty ImageFormatProperty = DependencyProperty.Register("ImageFormat", typeof(string), typeof(ImagePreview), new PropertyMetadata(""));
        
        public string ImageName
        {
            get => (string)GetValue(ImageNameProperty);
            set => SetValue(ImageNameProperty, value);
        }
        public static readonly DependencyProperty ImageNameProperty = DependencyProperty.Register("ImageName", typeof(string), typeof(ImagePreview), new PropertyMetadata(""));

        public ImagePreview()
        {
            InitializeComponent();
        }

        private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ImagePreview ip)) return;

            ip.NotifyImageChanged();
        }

        private void NotifyImageChanged()
        {
            N(nameof(PreviewImageWidth));
            N(nameof(PreviewImageHeight));
            ResetImageTransform();

        }

        private void OnImageMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var st = (ScaleTransform)((TransformGroup)ImageCtrl.RenderTransform).Children.First(tr => tr is ScaleTransform);
            var zoom = e.Delta > 0 ? .2 : -.2;
            if ((st.ScaleX + zoom) <= 0.1) return;
            st.ScaleX += zoom;
            st.ScaleY = st.ScaleX;
        }
        private void OnImageMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageCtrl.CaptureMouse();
            var tt = (TranslateTransform)((TransformGroup)ImageCtrl.RenderTransform).Children.First(tr => tr is TranslateTransform);
            _start = e.GetPosition(ImageContainer);
            _origin = new Point(tt.X, tt.Y);
        }
        private void OnImageMouseMove(object sender, MouseEventArgs e)
        {
            if (!ImageCtrl.IsMouseCaptured) return;
            var tt = (TranslateTransform)((TransformGroup)ImageCtrl.RenderTransform).Children.First(tr => tr is TranslateTransform);
            var v = _start - e.GetPosition(ImageContainer);
            tt.X = _origin.X - v.X;
            tt.Y = _origin.Y - v.Y;
        }
        private void OnImageMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageCtrl.ReleaseMouseCapture();
        }
        private void OnResetImageButtonClick(object sender, RoutedEventArgs e)
        {
            ResetImageTransform();
        }

        private void ResetImageTransform()
        {
            var tt = (TranslateTransform) ((TransformGroup) ImageCtrl.RenderTransform).Children.First(tr =>
                tr is TranslateTransform);
            var st = (ScaleTransform) ((TransformGroup) ImageCtrl.RenderTransform).Children.First(tr => tr is ScaleTransform);

            tt.X = 0;
            tt.Y = 0;

            st.ScaleX = 1;
            st.ScaleY = 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void N([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
