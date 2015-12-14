using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Command
{
    class DeleteNodeCommand : IUndoRedoCommand
    {
        private ObservableCollection<NodeViewModel> nodes;
        private List<NodeViewModel> selectedNodes = new List<NodeViewModel>();
        private ObservableCollection<LineViewModel> lines;
        private List<LineViewModel> removedLines = new List<LineViewModel>();

        public DeleteNodeCommand(ObservableCollection<NodeViewModel> _nodes, List<NodeViewModel> _selectedNodes, ObservableCollection<LineViewModel> _lines)
        {
            nodes = _nodes;
            foreach(NodeViewModel n in _selectedNodes)
            {
                selectedNodes.Add(n);
            }
                
            lines = _lines;
        }

        public override String ToString()
        {
            return "Delete node";
        }

        public void Execute()
        {
            foreach (NodeViewModel n in selectedNodes)
            {
                foreach (LineViewModel l in lines)
                {
                    if ((l.From == n || l.To == n) && !removedLines.Contains(l))
                    {
                        removedLines.Add(l);
                        l.From.removeNeighbour(l.To);
                        
                    }
                }
                nodes.Remove(n);
            }
            foreach (LineViewModel l in removedLines)
            {
                lines.Remove(l);
            }
        }

        public void UnExecute()
        {
            foreach (NodeViewModel n in selectedNodes)
            {
                nodes.Add(n);
            }
            foreach (LineViewModel l in removedLines)
            {
                lines.Add(l);
                l.From.addNeighbour(l.To);
                
            }
            removedLines.Clear();
        }
    }
}
