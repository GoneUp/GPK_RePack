using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPK_RePack.Model.Composite;

namespace GPK_RePack.Forms.Helper
{
    class CompositeTreeView : TreeView
    {
        private CompositeMap map;
        private string filter = "";
        public CompositeTreeView()
        {



        }

        public void SetMap(CompositeMap map)
        {
            this.map = map;
            OnDrawNodes();
        }

        public void FilterNodes(string text)
        {
            filter = text;
            OnDrawNodes();
        }

        private void OnDrawNodes()
        {
            this.BeginUpdate();
            this.Nodes.Clear();

            foreach (var entry in map.Map)
            {
                TreeNode filenode = new TreeNode(entry.Key);


                foreach (var subGPKs in entry.Value)
                {
                    var text = subGPKs.ToString();
                    if (filter == "" || text.Contains(filter))
                    {
                        filenode.Nodes.Add(subGPKs.ToString());
                    }
                }

                if (filenode.Nodes.Count > 0)
                {
                    this.Nodes.Add(filenode);
                }
            }

            this.EndUpdate();
        }



    }
}
