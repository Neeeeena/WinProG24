using AlgoTreeDraw.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgoTreeDraw.ViewModel
{
    public class Tree234 : Tree
    {
        List<LineViewModel> lines = new List<LineViewModel>();
        int offsetY { get; } = 70;
        public Tree234(List<NodeViewModel> selNodes) : base(selNodes)
        {
            if (allNodesT234())
            {
                
                nodes = splitAllNodes(nodes);
                nodes = nodes.OrderBy(n => int.Parse(((T234ViewModel)n).TxtOne)).ToList();

            }

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

        public Tuple<List<LineViewModel>, List<NodeViewModel>> BalanceT234()
        {
            removeLines();
            autobalance(nodes);
            double furthestLeft = 500;
            //For at træet ikke er inden under sidepanel
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
            return new Tuple<List<LineViewModel>, List<NodeViewModel>>(lines,nodes);
        }

        public void removeLines()
        {
            List<LineViewModel> tempLines = new List<LineViewModel>();
            foreach (NodeViewModel n in nodes)
            {
                foreach (LineViewModel l in Lines)
                {
                    if ((l.From == n || l.To == n)) tempLines.Add(l);
                }
            }

            foreach (LineViewModel l in tempLines)
            {
                Lines.Remove(l);
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

        private NodeViewModel autobalance(List<NodeViewModel> _nodes, double x = 0,double y=0, double offSetX = 300)
        {
            if(_nodes.Count >= 3)
            {
                var median = findMedian(_nodes);
                if(x != 0)
                {
                    median.X = x;
                    median.Y = y;
                }

                var leftMedian = findLeftMedian(_nodes);

                var rightMedian = findRightMedian(_nodes);

                ((T234ViewModel)median).Merge((T234ViewModel)leftMedian, (T234ViewModel)rightMedian);
                var left = autobalance(range(_nodes, null, leftMedian),median.X-offSetX/2,median.Y+offsetY,offSetX/2);
                if (left != null)
                {
                    lines.Add(new LineViewModel(new Line() { From = median.Node, To = left.Node }) { From = median, To = left });
                    median.addNeighbour(left);
                    left.X = median.X - offSetX;
                    left.Y = median.Y + offsetY; 
                }
                var leftright = autobalance(range(_nodes, leftMedian, median), median.X - offSetX / 4, median.Y + offsetY, offSetX / 2);
                if(leftright != null)
                {
                    lines.Add(new LineViewModel(new Line() { From = median.Node, To = leftright.Node }) { From = median, To = leftright });
                    median.addNeighbour(leftright);
                    leftright.X = median.X - offSetX/3;
                    leftright.Y = median.Y + offsetY;
                } 
                var rightleft = autobalance(range(_nodes, median, rightMedian), median.X + offSetX / 4, median.Y + offsetY, offSetX / 2);
                if(rightleft != null)
                {
                    lines.Add(new LineViewModel(new Line() { From = median.Node, To = rightleft.Node }) { From = median, To = rightleft });
                    median.addNeighbour(rightleft);
                    rightleft.X = median.X + offSetX/3;
                    rightleft.Y = median.Y + offsetY;
                }
                var right = autobalance(range(_nodes, rightMedian, null), median.X + offSetX / 2, median.Y + offsetY, offSetX / 2);
                if(right != null)
                {
                    lines.Add(new LineViewModel(new Line() { From = median.Node, To = right.Node }) { From = median, To = right });
                    median.addNeighbour(right);
                    right.X = median.X + offSetX;
                    right.Y = median.Y + offsetY;
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

                return left;
            }
            else if(_nodes.Count == 1)
            {
                NodeViewModel temp = _nodes.ElementAt(0);
                return temp;
            }
            return null;
        }

        //private void mergeNodes(List<LineViewModel> lines, NodeViewModel node)
        //{
        //    if (node != null)
        //    {
        //        var children = getChildren(node);
        //        if(children[0]!=null && children[1] != null)
        //        {
        //            ((T234ViewModel)root).Merge((T234ViewModel)children[0], (T234ViewModel)children[1]);
        //            var leftgrandchildren = getChildren(children[0]);
        //            var rightgrandchildren = getChildren(children[1]);
        //            nodes.Remove(children[0]);
        //            nodes.Remove(children[1]);
        //            lines.Remove(lines.Find(l => l.From == node && l.To == children[0]));
        //            lines.Remove(lines.Find(l => l.From == node && l.To == children[1]));
        //            lines.Add(new LineViewModel(new Line { From = node.Node, To = leftgrandchildren[0].Node }));
        //            lines.Add(new LineViewModel(new Line { From = node.Node, To = leftgrandchildren[1].Node }));
        //            lines.Add(new LineViewModel(new Line { From = node.Node, To = rightgrandchildren[0].Node }));
        //            lines.Add(new LineViewModel(new Line { From = node.Node, To = rightgrandchildren[1].Node }));
        //            mergeNodes(lines, leftgrandchildren[0]);
        //            mergeNodes(lines, leftgrandchildren[1]);
        //            mergeNodes(lines, rightgrandchildren[0]);
        //            mergeNodes(lines, rightgrandchildren[1]);
        //        }
        //        else if(children[1]==null && children[0] != null)
        //        {
        //            ((T234ViewModel)root).Merge((T234ViewModel)children[0]);
        //            var leftgrandchildren = getChildren(children[0]);
        //            nodes.Remove(children[0]);
        //            lines.Remove(lines.Find(l => l.From == node && l.To == children[0]));
        //            if(leftgrandchildren[0]!=null && leftgrandchildren[1] != null)
        //            {
        //                lines.Add(new LineViewModel(new Line { From = node.Node, To = leftgrandchildren[0].Node }));
        //                lines.Add(new LineViewModel(new Line { From = node.Node, To = leftgrandchildren[1].Node }));
        //                mergeNodes(lines, leftgrandchildren[0]);
        //                mergeNodes(lines, leftgrandchildren[1]);
        //            }
        //            if (leftgrandchildren[0] != null && leftgrandchildren[1] == null)
        //            {
        //                lines.Add(new LineViewModel(new Line { From = node.Node, To = leftgrandchildren[0].Node }));
        //                mergeNodes(lines, leftgrandchildren[0]);
        //            }


        //        }
                
                
        //    }
        //}


        //private Tuple<List<LineViewModel>, List<T234ViewModel>> balanceT234(List<LineViewModel> lines, List<T234ViewModel> _nodes)
        //{
        //var nodes = _nodes.OrderBy(n => n.TxtOne).ToList();
        //var mid = findMedian(nodes);
        //nodes.Remove(mid);
        //var leftList = nodes.GetRange(0, nodes.Count / 2);
        //var left = findMedian(leftList);
        //var rigthList = nodes.GetRange(nodes.Count / 2 + 1, nodes.Count);
        //var right = findMedian(rigthList);
        //leftList.Remove(left);
        //rigthList.Remove(right);
        //((T234)mid.Node).Merge((T234)left.Node, (T234)right.Node);




        //}

    }
}
