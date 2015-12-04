using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AlgoTreeDraw.Command
{
    class RemoveNodeInTreeCommand : IUndoRedoCommand
    {
        private ObservableCollection<NodeViewModel> nodes;
        private List<Tuple<string,int,Brush,Brush,Brush>> prevNodes = new List<Tuple<string, int, Brush, Brush, Brush>>();
        private List<NodeViewModel> selNodes = new List<NodeViewModel>();
        private List<LineViewModel> prevLines = new List<LineViewModel>();
        private ObservableCollection<LineViewModel> lines;
        private NodeViewModel removeThis;
        private List<LineViewModel> removedLines = new List<LineViewModel>();
        private Tree tree;

        public RemoveNodeInTreeCommand(Tree _tree, ObservableCollection<NodeViewModel> _nodes, List<NodeViewModel> _selectedNodes, ObservableCollection<LineViewModel> _lines)
        {
            nodes = _nodes;
            lines = _lines;
            tree = _tree;

            //foreach (NodeViewModel n in nodes)
            //{
            //    prevNodes.Add(new Tuple<string,int,Brush,Brush,Brush>(n.Key,n.ID,n.Color,n.ColorOfText,n.PreColor));
            //}
            foreach (NodeViewModel n in _selectedNodes)
                selNodes.Add(n);
        }

        public override String ToString()
        {
            return "Delete" + selNodes.ElementAt(0).Key + "tree";
        }

        public void Execute()
        {
            tree.nodes.Clear();
            tree.nodes.Add(selNodes.ElementAt(0));
            foreach (NodeViewModel n in nodes)
            {
                prevNodes.Add(new Tuple<string, int, Brush, Brush, Brush>(n.Key, n.ID, n.Color, n.ColorOfText, n.PreColor));
            }

            removeThis = tree.remove(selNodes.ElementAt(0));       
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
            
            nodes.Add(removeThis);
            foreach (Tuple<string, int, Brush, Brush, Brush> n in prevNodes) 
                foreach(NodeViewModel nn in nodes)
                    if(n.Item2 == nn.ID)
                    {
                        Console.WriteLine("ALRIGHT!! " + nn.Key + " " + n.Item1);
                        nn.Key = n.Item1;
                        nn.ID = n.Item2;
                        nn.Color = n.Item3;
                        nn.ColorOfText = n.Item4;
                        nn.PreColor = n.Item5;
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
