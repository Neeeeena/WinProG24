using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Command
{
    class InsertNodeInTreeCommand : IUndoRedoCommand
    {
        private ObservableCollection<NodeViewModel> nodes;
        private List<NodeViewModel> selNodes = new List<NodeViewModel>();
        private ObservableCollection<LineViewModel> lines;
        private NodeViewModel newNode;

        public InsertNodeInTreeCommand(ObservableCollection<NodeViewModel> _nodes, List<NodeViewModel> _selectedNodes, NodeViewModel _newNode, ObservableCollection<LineViewModel> _lines)
        {
            nodes = _nodes;
            lines = _lines;
            newNode = _newNode;
            foreach(NodeViewModel n in _selectedNodes)
            {
                selNodes.Add(n);
            }
        }
        
        public void Execute()
        {
            nodes.Add(newNode);
            Tree selTree = new Tree(selNodes);

            selTree.insert(newNode);
        }

        public void UnExecute()
        {
            nodes.Remove(newNode);
            foreach (LineViewModel l in lines)
            {
                if (l.From == newNode)
                {
                    lines.Remove(l);
                    newNode.removeNeighbour(l.To);
                    break;
                }
            }
        }
    }
}
