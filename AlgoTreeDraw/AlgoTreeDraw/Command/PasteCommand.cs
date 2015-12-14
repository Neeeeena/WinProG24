using AlgoTreeDraw.Model;
using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AlgoTreeDraw.Command
{
    class PasteCommand : IUndoRedoCommand
    {
        public ObservableCollection<NodeViewModel> Nodes { get; set; }
        public List<NodeViewModel> mostRecentPastedNodes = new List<NodeViewModel>();
        public List<NodeViewModel> selectedNodes = new List<NodeViewModel>();
        public List<NodeViewModel> copiedNodes = new List<NodeViewModel>();
        public ObservableCollection<LineViewModel> Lines = new ObservableCollection<LineViewModel>();
        public List<LineViewModel> copiedLines = new List<LineViewModel>();
        public List<LineViewModel> mostRecentPastedLines = new List<LineViewModel>();


        public PasteCommand(ObservableCollection<NodeViewModel> _nodes, List<NodeViewModel> _copiedNodes, List<NodeViewModel> _selectedNodes, List<LineViewModel> _copiedLines, ObservableCollection<LineViewModel> _lines)
        {
            Nodes = _nodes;
            selectedNodes = _selectedNodes;
            mostRecentPastedNodes = new List<NodeViewModel>();
            mostRecentPastedLines = new List<LineViewModel>();
            copiedNodes = _copiedNodes;
            Lines = _lines;
            copiedLines = _copiedLines;


        }

        public override String ToString()
        {
            return "Paste";
        }

        public void Execute()
        {
            clearSelectedNodes();
            foreach (NodeViewModel n in copiedNodes)
            {
                NodeViewModel node = n.newNodeViewModel();
                node.X += 30;
                node.Y += 30;
                Nodes.Add(node);
                addToSelectedNodes(node);
                mostRecentPastedNodes.Add(node);
            }

            foreach (LineViewModel l in copiedLines)
            {
                LineViewModel line = new LineViewModel(new Line() { From = l.From.Node, To = l.To.Node }) { From = l.From, To = l.To };
                foreach (NodeViewModel n in mostRecentPastedNodes)
                {
                    
                    if (line.To.X + 30 == n.X && line.To.Y + 30 == n.Y)
                    {
                        line.To = n;
                        
                    }
                    if (line.From.X + 30 == n.X && line.From.Y + 30 == n.Y)
                    {
                        line.From = n;
                        
                    }
                }
                Lines.Add(line);
                line.From.addNeighbour(line.To);
                mostRecentPastedLines.Add(line);

            }


        }

        public void UnExecute()
        {
            foreach (LineViewModel l in mostRecentPastedLines)
            {
                Lines.Remove(l);
                l.To.removeNeighbour(l.From);
            }
            mostRecentPastedLines.Clear();
            foreach (NodeViewModel n in mostRecentPastedNodes)
            {
                Nodes.Remove(n);
            }
            mostRecentPastedNodes.Clear();
        }

        public void addToSelectedNodes(NodeViewModel n)
        {
            selectedNodes.Add(n);

            n.BorderColor = Brushes.DarkBlue;
            n.BorderThickness = 4;
        }

        public void clearSelectedNodes()
        {
            foreach (NodeViewModel n in selectedNodes)
            {
                n.BorderColor = Brushes.Black;
                n.BorderThickness = 1;
            }
            selectedNodes.Clear();
        }
    }
}
