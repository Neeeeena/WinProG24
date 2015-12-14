using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgoTreeDraw.Command
{
    class AutoBalanceCommand : IUndoRedoCommand
    {
        private ObservableCollection<NodeViewModel> nodes;
        private List<NodeViewModel> selectedNodes = new List<NodeViewModel>();
        private ObservableCollection<LineViewModel> lines;
        private List<LineViewModel> autoBalanceLines = new List<LineViewModel>();
        private List<LineViewModel> originalLines = new List<LineViewModel>();
        private List<Point> nodePositions = new List<Point>();
        private Tree tree;
        
        public AutoBalanceCommand(Tree _tree, ObservableCollection<NodeViewModel> _nodes, List<NodeViewModel> _selectedNodes, ObservableCollection<LineViewModel> _lines)
        {
            tree = _tree;
            nodes = _nodes;
            lines = _lines;

            foreach (NodeViewModel n in _selectedNodes)
            {
                selectedNodes.Add(n);
                nodePositions.Add(new Point(n.X, n.Y));
                
                foreach (LineViewModel l in lines)
                {
                    if ((l.From == n || l.To == n) && !originalLines.Contains(l))
                    {
                        originalLines.Add(l);

                    }
                }
            }
            tree.nodes = new List<NodeViewModel>();
            foreach(NodeViewModel n in selectedNodes)
            {
                tree.nodes.Add(n);
            }
        }

        public void Execute()
        {
            
            autoBalanceLines = tree.tAutoBalance();
            
        }

        public void UnExecute()
        {
            int index = 0;
            if (autoBalanceLines != null)
            {
                // Slet lines og neighbours
                foreach(LineViewModel l in autoBalanceLines)
                {
                    lines.Remove(l);
                    l.From.removeNeighbour(l.To);
                }
            
                // Ryk nodes til deres originale position
                foreach(NodeViewModel n in selectedNodes)
                {
                    n.X = nodePositions.ElementAt(index).X;
                    n.Y = nodePositions.ElementAt(index).Y;
                    index++;
                }

                // Tilføje lines og neighbours
                foreach(LineViewModel l in originalLines)
                {
                    lines.Add(l);
                    l.From.addNeighbour(l.To);
                }

            }
            

        }
    }
}
