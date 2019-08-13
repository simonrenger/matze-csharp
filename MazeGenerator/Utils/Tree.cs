using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matze.Utils
{
    class Tree 
    {
        public Tree parent = null;
        public Tree root => (parent != null)? parent.root : this;

        public bool IsConnected(Tree tree)
        {
            return root == tree.root;
        }

        public void Connect(Tree tree)
        {
            tree.root.parent = this;
        }
    }
}
