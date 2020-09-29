using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GPK_RePack.Core.Model;

namespace GPK_RePack_WPF.Windows
{
    /// <summary>
    /// Interaction logic for MapperWindow.xaml
    /// </summary>
    public partial class MapperWindow : Window
    {
        public MapperWindow(GpkStore gpkStore)
        {
            InitializeComponent();
        }
    }
}
