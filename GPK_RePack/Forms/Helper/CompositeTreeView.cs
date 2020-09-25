using System.Windows.Forms;
using GPK_RePack.Core.Model;

namespace GPK_RePack.Forms.Helper
{
    class CompositeTreeView : TreeView
    {
        private GpkStore store;
        private string filter = "";
        private TreeNode[] fullCachedTree = null;
        public CompositeTreeView()
        {



        }

        public void SetStore(GpkStore store)
        {
            this.store = store;
            OnDrawNodes();
        }

        public void FilterNodes(string text)
        {
            filter = text.ToLower();
            OnDrawNodes();
        }

        public void OnDrawNodes()
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
                foreach (var entry in store.CompositeMap)
                {
                    TreeNode filenode = new TreeNode(entry.Key);


                    foreach (var subGPK in entry.Value)
                    {
                        var text = subGPK.ToString();
                        if (filter == "" || text.ToLower().Contains(filter))
                        {
                            TreeNode subNode = new TreeNode(subGPK.ToString());
                            subNode.Tag = subGPK;
                            filenode.Nodes.Add(subNode);
                            
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
