using AlgoTreeDraw.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.ViewModel
{
    public class BSTViewModel : NodeViewModel
    {
        public BSTViewModel(Node _node) : base(_node) { }

        

        public override NodeViewModel newNodeViewModel()
        {
            return new BSTViewModel(new BST());
        }


    }
}
