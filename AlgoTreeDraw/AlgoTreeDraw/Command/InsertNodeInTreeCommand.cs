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
        private NodeViewModel nvm;
        private List<NodeViewModel> selNodes = new List<NodeViewModel>();
        private ObservableCollection<LineViewModel> lines;

        public InsertNodeInTreeCommand(ObservableCollection<NodeViewModel> _nodes, List<NodeViewModel> _selectedNodes, NodeViewModel _nvm, ObservableCollection<LineViewModel> _lines)
        {
            nodes = _nodes;
            lines = _lines;
            nvm = _nvm;
            foreach(NodeViewModel n in _selectedNodes)
            {
                selNodes.Add(n);
            }
        }
        
        public void Execute()
        {
            Tree selTree = new Tree(selNodes);
            selTree.insert(nvm);
        }

        public void UnExecute()
        {
            nodes.Remove(nvm);
            foreach(LineViewModel l in lines)
            {
                if(l.From == nvm)
                {
                    lines.Remove(l);
                    nvm.removeNeighbour(l.To);
                    break;
                }
            }
        }
    }
}
