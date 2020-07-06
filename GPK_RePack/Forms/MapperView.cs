using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
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

            IDisposable subscription = 
                Observable
                .FromEventPattern(
                    h => boxSearch.TextChanged += h,
                    h => boxSearch.TextChanged -= h)
                .Select(x => boxSearch.Text)
                .Throttle(TimeSpan.FromMilliseconds(300))
                .Select(x => Observable.Start(() => x))
                .Switch()
                .ObserveOn(this)
                .Subscribe(x => boxSearch_TextChanged());
        }

        private void formMapperView_Load(object sender, EventArgs e)
        {

        }

        private void treeMapperView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }


        private void boxSearch_TextChanged()
        {
            if (boxSearch.Text.Length == 0 || boxSearch.Text.Length > 3)
            {
                treeMapperView.FilterNodes(boxSearch.Text);
            }
        }

        private void boxSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
