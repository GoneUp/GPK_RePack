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
        private TreeNode[] fullCachedTree = null;
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
            filter = text.ToLower();
            OnDrawNodes();
        }

        private void OnDrawNodes()
        {
            this.BeginUpdate();
            this.Nodes.Clear();

            if (filter == "" && fullCachedTree != null)
            {
                this.Nodes.AddRange(fullCachedTree);
            }
            else
            {
                //filter or build cache
                foreach (var entry in map.Map)
                {
                    TreeNode filenode = new TreeNode(entry.Key);


                    foreach (var subGPKs in entry.Value)
                    {
                        var text = subGPKs.ToString();
                        if (filter == "" || text.ToLower().Contains(filter))
                        {
                            filenode.Nodes.Add(subGPKs.ToString());
                        }
                    }

                    if (filenode.Nodes.Count > 0)
                    {
                        this.Nodes.Add(filenode);
                    }
                }
            }


            if (filter == "" && fullCachedTree == null)
            {
                fullCachedTree = new TreeNode[this.Nodes.Count];
                this.Nodes.CopyTo(fullCachedTree, 0);
            }

            this.EndUpdate();
        }



    }
}
