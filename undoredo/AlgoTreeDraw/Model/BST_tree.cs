using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Model
{
    class BST_tree
    {
        public List<BSTViewModel> tree = new List<BSTViewModel>();

        public BST_tree(List<BSTViewModel> tree)
        {
            this.tree = tree;
        }

        public BSTViewModel search(BSTViewModel root, int key)
        {
            if (root == null || root.key == key) return root;
            if (root.key < key) return search(ro);
        }


    }
}
