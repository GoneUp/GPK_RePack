using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Nostrum.Extensions;

namespace GPK_RePack_WPF.Windows
{
    //todo: rework this in a proper way
    public partial class InputBoxWindow : Window
    {
        public string ReturnValue { get; set; } = "";

        public InputBoxWindow(string msg)
        {
            InitializeComponent();
            Dispatcher.InvokeIfRequired(() =>
            {
                if (App.Current.MainWindow != null) Owner = App.Current.MainWindow;
                Hint.Text = msg;
                Input.Focus();
                Input.KeyDown += OnKeyDown;
            }, DispatcherPriority.DataBind);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) Close();
            else if (e.Key == Key.Escape)
            {
                ReturnValue = "";
                Close();
            }
        }

        public new string ShowDialog()
        {
            base.ShowDialog();
            return ReturnValue;
        }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
