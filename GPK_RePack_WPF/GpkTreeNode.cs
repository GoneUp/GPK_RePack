using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using GPK_RePack.Core.Model;
using Nostrum;

namespace GPK_RePack_WPF
{
    public class GpkTreeNode : TSPropertyChanged
    {
        // ugly ref for binding
        public MainViewModel MainVM => MainViewModel.Instance;

        public string Key { get; set; }
        public string Name { get; set; }
        public string KeyNoName
        {
            get
            {
                if (Key == Name) return "";
                var split = Key.Split('.').ToList();
                split.Remove(split.Last());
                return string.Join(".", split);
            }
        }
        public string Class { get; set; }
        public bool IsPackage { get; set; }
        //public bool IsExport { get; set; }
        public bool IsImport { get; set; }
        public bool IsClass { get; set; }
        public int Index { get; set; }
        public TSObservableCollection<GpkTreeNode> Children { get; set; }
        public IComparer TreeViewNodeSorter { get; set; }
        public GpkTreeNode Parent { get; set; }

        private bool _isSelected;
        private bool _isExpanded;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                N();
            }
        }
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                N();
            }
        }

        public int Level
        {
            get
            {
                if (Parent == null) return 0;
                return Parent.Level + 1;
            }
        }

        public bool IsLeaf => Children.Count == 0;
        public GpkPackage SourcePackage { get; }

        public List<GpkTreeNode> ParentsToPackage
        {
            get
            {
                var ret = new List<GpkTreeNode>();

                ret.Add(this);
                if (this.IsPackage)
                {
                    return ret;
                }

                ret.AddRange(Parent.ParentsToPackage);

                return ret;
            }
        }

        public override string ToString()
        {
            return $"[{Level}] {Key}";
        }

        public GpkTreeNode(string name)
        {
            Children = new TSObservableCollection<GpkTreeNode>();
            Name = name;
            Key = name;
        }
        public GpkTreeNode(string name, string key) : this(name)
        {
            Key = key;
        }
        public GpkTreeNode(string name, GpkPackage src) : this(name)
        {
            SourcePackage = src;
        }

        public void AddNode(GpkTreeNode node)
        {
            node.Parent = this;
            Children.Add(node);
        }

        public GpkTreeNode FindPackageNode()
        {
            return IsPackage ? this : Parent?.FindPackageNode();
        }

        public static void CheckClassNode(string className, Dictionary<string, GpkTreeNode> classNodes, GpkTreeNode mainNode)
        {
            if (!className.Contains("."))
            {
                //base case
                if (!classNodes.ContainsKey(className))
                {
                    var classNode = new GpkTreeNode(className);
                    mainNode.AddNode(classNode);
                    classNodes.Add(className, classNode);
                }
            }
            else
            {
                var split = className.Split('.').ToList();
                var toAdd = split.Last();
                split.RemoveAt(split.Count - 1);
                var left = String.Join(".", split);

                //recursion to add missing nodes
                if (!classNodes.ContainsKey(left))
                {
                    CheckClassNode(left, classNodes, mainNode);
                }

                if (!classNodes.ContainsKey(className))
                {
                    var classNode = new GpkTreeNode(toAdd);
                    classNodes[left].AddNode(classNode);
                    classNodes.Add(className, classNode);
                }
            }

        }

        public void Remove()
        {
            Children.Clear();
            this.Parent.Children.Remove(this);
        }

        /// <summary>
        /// Returns all the descendants of this node.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GpkTreeNode> Collect()
        {
            foreach (var node in Children)
            {
                yield return node;

                foreach (var child in node.Collect())
                    yield return child;
            }
        }
    }
}