using AlgoTreeDraw.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.ViewModel
{
    class T234ViewModel : NodeViewModel
    {
        public T234ViewModel(Node _node) : base(_node) { }

        public override NodeViewModel newNodeViewModel()
        {
            return new T234ViewModel(new T234());
        }
    }
}
