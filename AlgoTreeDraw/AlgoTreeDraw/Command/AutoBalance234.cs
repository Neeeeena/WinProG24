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
    public class AutoBalance234 : IUndoRedoCommand
    {
        private ObservableCollection<NodeViewModel> nodes;
        private List<NodeViewModel> selectedNodes = new List<NodeViewModel>();
        private List<NodeViewModel> afterNodes = new List<NodeViewModel>();
        private ObservableCollection<LineViewModel> lines;
        private List<LineViewModel> autoBalanceLines = new List<LineViewModel>();
        private List<LineViewModel> originalLines = new List<LineViewModel>();
        private List<NodeViewModel> originalNodes = new List<NodeViewModel>();
        private List<Point> nodePositions = new List<Point>();

        public AutoBalance234(ObservableCollection<NodeViewModel> _nodes, List<NodeViewModel> _selectedNodes, ObservableCollection<LineViewModel> _lines)
        {
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
                originalNodes.Add(n.newNodeViewModel());
            }

        }

        public override string ToString()
        {
            return "Autobalance 2-3-4-Tree";
        }

        public void Execute()
        {
            foreach(var n in selectedNodes)
            {
                nodes.Remove(n);
            }
            selectedNodes.Clear();
            //copy all nodes to selectednodes
            foreach(var n in originalNodes)
            {
                selectedNodes.Add(n);
            }
            originalNodes.Clear();
            foreach (var n in selectedNodes)
            {
                nodes.Remove(n);
                originalNodes.Add(n.newNodeViewModel());
            }

            Tree234 tree = new Tree234(selectedNodes);
            Tuple<List<LineViewModel>, List<NodeViewModel>> tuple = tree.BalanceT234();
            afterNodes = tuple.Item2;
            nodes.AddRange(afterNodes);
            autoBalanceLines = tuple.Item1;
            lines.AddRange(autoBalanceLines);

        }

        public void UnExecute()
        {
            int index = 0;
            if (autoBalanceLines != null)
            {
                // Slet lines og neighbours
                foreach (LineViewModel l in autoBalanceLines)
                {
                    lines.Remove(l);
                    l.From.removeNeighbour(l.To);
                }

                foreach (NodeViewModel n in afterNodes)
                {
                    nodes.Remove(n);
                }

                // Tilføj og ryk nodes til deres originale position
                foreach (NodeViewModel n in originalNodes)
                {
                    nodes.Add(n);
                    n.X = nodePositions.ElementAt(index).X;
                    n.Y = nodePositions.ElementAt(index).Y;
                    index++;
                }

                // Tilføje lines og neighbours
                foreach (LineViewModel l in originalLines)
                {
                    lines.Add(l);
                    l.From.addNeighbour(l.To);
                }

            }

        }
    }
}
