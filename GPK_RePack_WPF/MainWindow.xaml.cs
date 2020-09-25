using System;
using Nostrum.Extensions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Nostrum;

namespace GPK_RePack_WPF
{
    public class MainViewModel : TSPropertyChanged
    {
    }
    public partial class MainWindow : Window
    {
        public ICommand CloseCommand { get; set; }
        public ICommand MinimizeCommand { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            CloseCommand = new RelayCommand(_ =>
            {
                this.Close();
            });

            MinimizeCommand = new RelayCommand(_ =>
            {
                WindowState = WindowState.Minimized;
            });

            DataContext = new MainViewModel();
            this.StateChanged += OnStateChanged;
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            MainContent.Margin = WindowState == WindowState.Maximized ? new Thickness(8) : new Thickness(0);
        }


        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                var currLeft = Left;
                var currTop = Top;
                var posX = Mouse.GetPosition(this).X;
                var posY = Mouse.GetPosition(this).Y;
                WindowState = WindowState.Normal;
                Left = currLeft - 8 + 0;
                Top = -13;
            }
            this.TryDragMove();
        }
    }
}
