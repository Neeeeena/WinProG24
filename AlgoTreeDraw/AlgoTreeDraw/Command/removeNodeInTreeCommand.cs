using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Command
{
    class RemoveNodeInTreeCommand : IUndoRedoCommand
    {
        private ObservableCollection<NodeViewModel> nodes;
        private List<Tuple<string,int>> prevNodes = new List<Tuple<string, int>>();
        private List<NodeViewModel> selNodes = new List<NodeViewModel>();
        private List<LineViewModel> prevLines = new List<LineViewModel>();
        private ObservableCollection<LineViewModel> lines;
        private NodeViewModel removeThis;
        private List<LineViewModel> removedLines = new List<LineViewModel>();

        public RemoveNodeInTreeCommand(ObservableCollection<NodeViewModel> _nodes, List<NodeViewModel> _selectedNodes, ObservableCollection<LineViewModel> _lines)
        {
            nodes = _nodes;
            lines = _lines;

            foreach (NodeViewModel n in _nodes)
            {
                prevNodes.Add(new Tuple<string,int>(n.Key,n.ID));

            }
            foreach (NodeViewModel n in _selectedNodes)
                selNodes.Add(n);
        }

        public void Execute()
        {
            Tree selTree = new Tree(selNodes);
            removeThis = selTree.remove(selNodes.ElementAt(0).Key);       
            foreach (LineViewModel l in lines)
            {
                if ((l.From == removeThis || l.To == removeThis) && !removedLines.Contains(l))
                {
                    removedLines.Add(l);
                    l.From.removeNeighbour(l.To);
                }
            }
            nodes.Remove(removeThis);
            foreach (LineViewModel l in removedLines)
            {
                lines.Remove(l);
            }
        }

        public void UnExecute()
        {
            {
                nodes.Add(removeThis);
                foreach (Tuple<string, int> n in prevNodes) 
                    foreach(NodeViewModel nn in nodes)
                        if(n.Item2 == nn.ID)
                        {
                            nn.Key = n.Item1;
                            Console.WriteLine("ALRIGHT!! " + nn.Key + " " + n.Item1);
                        }
                foreach (LineViewModel l in removedLines)
                {
                    lines.Add(l);
                    l.From.addNeighbour(l.To);
                }
            }
        }


    }
}
