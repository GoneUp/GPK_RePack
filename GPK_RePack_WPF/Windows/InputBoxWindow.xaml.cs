using System.Windows;
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
                Hint.Text = msg;
            }, DispatcherPriority.DataBind);
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
