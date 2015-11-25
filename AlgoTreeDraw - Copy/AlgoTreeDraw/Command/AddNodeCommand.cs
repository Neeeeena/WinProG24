using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Command
{
    class AddNodeCommand : IUndoRedoCommand
    {
        private ObservableCollection<NodeViewModel> nodes;
        private NodeViewModel node;

        public AddNodeCommand(ObservableCollection<NodeViewModel> _nodes, NodeViewModel _node)
        {
            nodes = _nodes;
            node = _node;
        }

        public void Execute()
        {
            nodes.Add(node);
        }

        public void UnExecute()
        {
            nodes.Remove(node);
        }
    }
}
