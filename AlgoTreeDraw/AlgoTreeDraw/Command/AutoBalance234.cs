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
        private readonly List<NodeViewModel> selectedNodes = new List<NodeViewModel>();
        private ObservableCollection<LineViewModel> lines;
        private List<NodeViewModel> autoBalanceNodes = new List<NodeViewModel>();
        private List<LineViewModel> originalLines = new List<LineViewModel>();
        private List<NodeViewModel> executeNodes = new List<NodeViewModel>();
        private List<LineViewModel> executeLines = new List<LineViewModel>();

        public AutoBalance234(ObservableCollection<NodeViewModel> _nodes, List<NodeViewModel> _selectedNodes, ObservableCollection<LineViewModel> _lines)
        {
            nodes = _nodes;
            lines = _lines;

            

            foreach (NodeViewModel n in _selectedNodes)
            {
                selectedNodes.Add(n);

                foreach (LineViewModel l in lines)
                {
                    if ((l.From == n || l.To == n) && !originalLines.Contains(l))
                    {
                        originalLines.Add(l);
                        executeLines.Add(l);

                    }
                }
            }

        }

        public override string ToString()
        {
            return "Autobalance 2-3-4-Tree";
        }

        public void Execute()
        {
            foreach(var l in originalLines)
            {
                lines.Remove(l);
            }
            foreach(var l in executeLines)
            {
                lines.Remove(l);
            }
            foreach(var n in executeNodes)
            {
                nodes.Remove(n);   
            }
            executeNodes.Clear();

            foreach(var n in selectedNodes)
            {
                executeNodes.Add(n.newNodeViewModel());
                nodes.Remove(n);
            }

            Tree234 tree = new Tree234(executeNodes, lines);
            executeNodes = tree.BalanceT234();
            nodes.AddRange(executeNodes);
            

        }

        public void UnExecute()
        {

            removeLines();
            foreach (NodeViewModel n in executeNodes)
            {
                nodes.Remove(n);
            }

            foreach (NodeViewModel n in selectedNodes)
            {
                nodes.Add(n);
            }

                // Tilføje lines og neighbours
            foreach (LineViewModel l in originalLines)
            {
                lines.Add(l);
             }

        }

        public void removeLines()
        {
            List<LineViewModel> tempLines = new List<LineViewModel>();
            foreach (NodeViewModel n in nodes)
            {
                foreach (LineViewModel l in lines)
                {
                    if ((l.From == n || l.To == n)) tempLines.Add(l);
                }
            }

            foreach (LineViewModel l in tempLines)
            {
                lines.Remove(l);
            }
        }

    }
}
