using System.Windows;
using System.Windows.Controls;

namespace GPK_RePack_WPF.Controls
{
    public static class TextBoxExtensions 
    {
        public static readonly DependencyProperty AutoScrollEnabledProperty = DependencyProperty.RegisterAttached(
            "AutoScrollEnabled", 
            typeof(bool), 
            typeof(TextBoxExtensions), 
            new UIPropertyMetadata(false, OnAutoScrollEnabledChanged));

        private static void OnAutoScrollEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBox tb)) return;
            var enabled = e.NewValue is bool b && b;

            if (enabled)
            {
                tb.TextChanged += OnTextChanged;
            }
            else
            {
                tb.TextChanged -= OnTextChanged;
            }
        }

        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox tb)) return;
            tb.ScrollToEnd();
        }

        public static bool GetAutoScrollEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollEnabledProperty);
        }

        public static void SetAutoScrollEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollEnabledProperty, value);
        }

    }
}
