using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matze.Utils
{
    class Tree
    {
        public Tree root = null;

        public Tree()
        {
            root = this;
        }
        public bool IsConnected(ref Tree tree)
        {
            return tree.root == this;
        }

        public void Connect(ref Tree tree)
        {
            root = tree.root;
        }
    }
}
