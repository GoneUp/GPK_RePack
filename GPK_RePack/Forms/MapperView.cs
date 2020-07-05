using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPK_RePack.Model.Composite;

namespace GPK_RePack.Forms
{
    public partial class formMapperView : Form
    {
        public formMapperView(CompositeMap map)
        {
            InitializeComponent();

            this.treeMapperView.SetMap(map);
        }

        private void formMapperView_Load(object sender, EventArgs e)
        {

        }

        private void treeMapperView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void boxSearch_TextChanged(object sender, EventArgs e)
        {
            treeMapperView.FilterNodes(boxSearch.Text);
        }
    }
}
