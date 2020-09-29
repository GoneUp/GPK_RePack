using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GPK_RePack.Core;
using NLog;
using Nostrum;
using Nostrum.Extensions;

namespace GPK_RePack_WPF.Windows
{

    public partial class MainWindow : Window
    {
        private readonly Logger logger;

        public ICommand CloseCommand { get; set; }
        public ICommand MinimizeCommand { get; set; }
        public ICommand OpenSettingsCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            

            logger = LogManager.GetLogger("GUI");

            CloseCommand = new RelayCommand(_ =>
            {
                logger.Info("Shutdown");
                if (DataContext is IDisposable dc) dc.Dispose();
                CoreSettings.Save();
                LogManager.Flush();
                Close();
            });

            MinimizeCommand = new RelayCommand(_ =>
            {
                WindowState = WindowState.Minimized;
            });

            OpenSettingsCommand = new RelayCommand(_ =>
            {
                new SettingsWindow().ShowDialog();
            });

            DataContext = new MainViewModel();
            ((TSPropertyChanged)DataContext).PropertyChanged += OnDcPropChanged; // todo: make a custom auto-scroll textbox control and remove this
            StateChanged += OnStateChanged;
        }

        private void OnDcPropChanged(object sender, PropertyChangedEventArgs e)
        {

        }


        private void OnStateChanged(object sender, EventArgs e)
        {
            MainContent.Margin = WindowState == WindowState.Maximized ? new Thickness(6) : new Thickness(0);
        }


        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                var point = PointToScreen(e.MouseDevice.GetPosition(this));
                WindowState = WindowState.Normal;
                Left = point.X - RestoreBounds.Width * 0.5;
                Top = -13;
            }
            this.TryDragMove();
        }

        private void OnTreeViewDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                logger.Debug("Drop input: " + file);
            }

            ((MainViewModel)DataContext).OpenCommand.Execute(files);
        }

        private void OnSelectedNodeChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is GpkTreeNode n)
                ((MainViewModel)DataContext).SelectNode(n);
        }

        private void OnTextureButtonClick(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 2;
        }


        private void OnTreeNodeMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is GpkTreeNode n && e.RightButton == MouseButtonState.Pressed)
            {
                ((MainViewModel)DataContext).SelectNode(n);
                ((FrameworkElement)sender).FindVisualParent<TreeViewItem>().Focus();
            }

        }
    }
}
