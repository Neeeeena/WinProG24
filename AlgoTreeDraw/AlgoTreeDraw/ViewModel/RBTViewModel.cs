using AlgoTreeDraw.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.ViewModel
{
    class RBTViewModel : NodeViewModel
    {
        public RBTViewModel(Node _node) : base(_node) { }

        public override NodeViewModel newNodeViewModel()
        {
            return new RBTViewModel(new RBT());
        }
    }
}
