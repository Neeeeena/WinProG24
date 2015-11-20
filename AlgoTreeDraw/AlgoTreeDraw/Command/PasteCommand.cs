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
        
        public PasteCommand(ObservableCollection<NodeViewModel> Nodes, List<NodeViewModel> copiedNodes, List<NodeViewModel> selectedNodes, List<NodeViewModel> mostRecentPastedNodes)
        {
            this.Nodes = Nodes;
            this.selectedNodes = selectedNodes;
            this.mostRecentPastedNodes = mostRecentPastedNodes;
            this.copiedNodes = copiedNodes;
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
        }

        public void UnExecute()
        {
            foreach (NodeViewModel n in mostRecentPastedNodes)
            {
                Nodes.Remove(n);
            }
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
