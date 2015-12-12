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
    class MakePrettyCommand : IUndoRedoCommand
    {
        private ObservableCollection<NodeViewModel> nodes;
        private List<NodeViewModel> oldNodes = new List<NodeViewModel>();
        private List<NodeViewModel> selectedNodes = new List<NodeViewModel>();
        private List<Point> nodePositions = new List<Point>();

        public MakePrettyCommand(ObservableCollection<NodeViewModel> _nodes, List<NodeViewModel> _selectedNodes)
        {
            nodes = _nodes;
            selectedNodes = _selectedNodes;
            foreach (NodeViewModel n in _nodes)
            {
                nodePositions.Add(new Point(n.X, n.Y));
            }         
        }

        public override String ToString()
        {
            return "Align";
        }

        public void Execute()
        {
            Tree selTree = new Tree(selectedNodes);
            selTree.makePretty(false);
        }

        public void UnExecute()
        {
            int index = 0;
            foreach (NodeViewModel n in nodes)
            {
                n.X = nodePositions.ElementAt(index).X;
                n.Y = nodePositions.ElementAt(index).Y;
                index++;
            }
        }
    }
}
