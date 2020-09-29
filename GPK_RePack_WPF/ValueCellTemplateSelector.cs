using System.Windows;
using System.Windows.Controls;

namespace GPK_RePack_WPF
{
    public class ValueCellTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextTemplate { get; set; }
        public DataTemplate EnumTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is PropertyViewModel row)) return TextTemplate;
            return row.EditAsEnum ? EnumTemplate : TextTemplate;
        }
    }
}