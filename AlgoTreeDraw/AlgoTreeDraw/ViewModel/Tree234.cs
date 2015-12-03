using AlgoTreeDraw.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgoTreeDraw.ViewModel
{
    public class Tree234 
    {
        ObservableCollection<LineViewModel> lines;
        List<NodeViewModel> nodes = new List<NodeViewModel>();
        int offsetY { get; } = 70;
        double zeroHeight = 0;
        double zeroLenght = 0;
        public Tree234(List<NodeViewModel> selNodes, ObservableCollection<LineViewModel> lines)
        {
            if(selNodes.Count != 0)
            {
                nodes = selNodes;
                if (allNodesT234() && allNodesIntKeys())
                {

                    nodes = splitAllNodes(nodes);
                    nodes = nodes.OrderBy(n => int.Parse(((T234ViewModel)n).TxtOne)).ToList();

                    this.lines = lines;

                }


            }
           
            


        }
        public bool testValidTree()
        {
            return allNodesIntKeys() && allNodesT234();
        }

        private bool allNodesIntKeys()
        {
            int throwaway;
            foreach (NodeViewModel n in nodes)
            {
                if (!int.TryParse(((T234ViewModel)n).TxtOne, out throwaway))
                {
                    MessageBox.Show("Error: Some nodes does not have valid values (numbers)");

                    return false;
                }
            }
            return true;
        }



        private bool allNodesT234()
        {
            foreach (NodeViewModel n in nodes)
            {
                if (!(n is T234ViewModel))
                {
                    MessageBox.Show("Error: All nodes are not T234-nodes");
                    return false;
                }
            }
            return true;

        }

        private List<NodeViewModel> splitAllNodes(List<NodeViewModel> treeList)
        {
            List<NodeViewModel> splitList = new List<NodeViewModel>();
            foreach (T234ViewModel n in treeList)
            {
                //Splits the node into two nodes
                var temp = n.Split();
                splitList.AddRange(temp);
                splitList.Add(n);

            }
            return splitList;
        }

        private NodeViewModel findMedian(List<NodeViewModel> treeList)
        {
            return treeList.ElementAt(treeList.Count/2);
        }
        private NodeViewModel findLeftMedian(List<NodeViewModel> treeList)
        {
            return treeList.ElementAt(treeList.Count / 4);
        }
        private NodeViewModel findRightMedian(List<NodeViewModel> treeList)
        {
            return treeList.ElementAt(3*treeList.Count / 4);
        }

        public List<NodeViewModel> BalanceT234()
        {
            removeLines();
            autobalance(nodes);
            double furthestLeft = 500;
            //For at træet ikke er inden under sidepanel
            align();
            foreach(var n in nodes)
            {
                if (n.X < furthestLeft)
                {
                    furthestLeft = n.X;
                }
            }
            if (furthestLeft < 150 && furthestLeft > 0)
            {
                foreach (var n in nodes)
                {
                    n.X += furthestLeft;
                }
            }
            else if(furthestLeft <= 0)
            {
                foreach (var n in nodes)
                {
                    n.X += (-furthestLeft)+150;
                }
            }
            //Slut på for at træet ikke er inden under sidepanel
            return nodes;
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

        //Gets the elements between the two elements, both not included
        private List<NodeViewModel> range(List<NodeViewModel> _nodes, NodeViewModel n1 = null, NodeViewModel n2 = null)
        {
            int index1 = 0;
            int index2 = 0;
            if(n1 != null && n2 != null)
            {
                index1 = _nodes.IndexOf(n1)+1;
                index2 = _nodes.IndexOf(n2)-1;
            }
            else if(n1 != null && n2 == null)
            {
                index1 = _nodes.IndexOf(n1)+1;
                index2 = _nodes.Count - 1;
            }
            else if (n1 == null && n2 != null)
            {
                index2 = _nodes.IndexOf(n2)-1; 
            }
            return _nodes.GetRange(index1, index2 - index1+1);

        }

        private NodeViewModel autobalance(List<NodeViewModel> _nodes, double y=0)
        {
            if(_nodes.Count >= 3)
            {
                var median = findMedian(_nodes);
                if(y != 0)
                {
                    median.X = zeroLenght;
                    median.Y = y;
                }
                else
                {
                    zeroHeight = median.Y;
                    zeroLenght = median.X + 1.5*((T234ViewModel)median).Length()+5;
                }

                var leftMedian = findLeftMedian(_nodes);

                var rightMedian = findRightMedian(_nodes);

                ((T234ViewModel)median).Merge((T234ViewModel)leftMedian, (T234ViewModel)rightMedian);
                var left = autobalance(range(_nodes, null, leftMedian),median.Y+offsetY);
                if (left != null)
                {
                    lines.Add(new LineViewModel(new Line() { From = median.Node, To = left.Node }) { From = median, To = left });
                    median.addNeighbour(left);

                }
                var leftright = autobalance(range(_nodes, leftMedian, median), median.Y + offsetY);
                if(leftright != null)
                {
                    lines.Add(new LineViewModel(new Line() { From = median.Node, To = leftright.Node }) { From = median, To = leftright });
                    median.addNeighbour(leftright);
                } 
                var rightleft = autobalance(range(_nodes, median, rightMedian), median.Y + offsetY);
                if(rightleft != null)
                {
                    lines.Add(new LineViewModel(new Line() { From = median.Node, To = rightleft.Node }) { From = median, To = rightleft });
                    median.addNeighbour(rightleft);

                }
                var right = autobalance(range(_nodes, rightMedian, null), median.Y + offsetY);
                if(right != null)
                {
                    lines.Add(new LineViewModel(new Line() { From = median.Node, To = right.Node }) { From = median, To = right });
                    median.addNeighbour(right);

                }
                
                nodes.Remove(leftMedian);
                nodes.Remove(rightMedian);

                return median;
            }
            else if(_nodes.Count == 2)
            {
                var left = (T234ViewModel)_nodes.ElementAt(0);
                var right = (T234ViewModel)_nodes.ElementAt(1);
                left.Merge(right);
                nodes.Remove(right);
                left.X = zeroLenght;
                left.Y = y;

                return left;
            }
            else if(_nodes.Count == 1)
            {
                NodeViewModel one = _nodes.ElementAt(0);
                one.X = zeroLenght;
                one.Y = y;
                return one;
            }
            return null;
        }

        private void align()
        {
            int offset = 70;
            while (true)
            {
                List<NodeViewModel> level = nodes.FindAll(n => n.Y == zeroHeight + offset);
                if(level.Count != 0)
                {
                    level.OrderBy(n => int.Parse(((T234ViewModel)n).TxtOne));
                    level.ElementAt(level.Count / 2 + 1).X = zeroLenght;
                    for(int i = level.Count/2; i <= level.Count-2; i++)
                    {
                        var left = ((T234ViewModel)level.ElementAt(i));
                        var right = ((T234ViewModel)level.ElementAt(i+1));
                        if (left.X+left.Length()>= right.X)
                        {
                            right.X = left.X + left.Length() + 10;
                        }
                    }
                    for (int i = level.Count / 2; i > 0; i--)
                    {
                        var left = ((T234ViewModel)level.ElementAt(i-1));
                        var right = ((T234ViewModel)level.ElementAt(i));
                        if (right.X <= left.X+left.Length())
                        {
                            left.X = right.X -right.Length() - 10;
                        }
                    }
                    offset += 70;

                }
                else
                {
                    break;
                }
                

            }

        }


    }
}
