using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPK_RePack.Model;
using GPK_RePack.Model.Composite;
using NLog;

namespace GPK_RePack.Forms
{
    partial class FormMapperView : Form
    {
        private GpkStore store;
        private Logger logger = LogManager.GetCurrentClassLogger();
        public FormMapperView(GpkStore store)
        {
            InitializeComponent();

            this.store = store;
            treeMapperView.SetStore(store);

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
            if (e.Node == null || e.Node.Tag == null)
                return;

            CompositeMapEntry entry = (CompositeMapEntry)e.Node.Tag;
            string path = string.Format("{0}\\{1}.gpk", store.BaseSearchPath, entry.SubGPKName);

            if (!File.Exists(path))
            {
                logger.Info("GPK to load not found");
                return;
            }
            store.loadSubGpk(path, entry);
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

        private void btnDeleteMapping_Click(object sender, EventArgs e)
        {
            this.Close();
            this.store.clearCompositeMap();
            logger.Info("Deleted Mapping Cache");
        }
    }
}
