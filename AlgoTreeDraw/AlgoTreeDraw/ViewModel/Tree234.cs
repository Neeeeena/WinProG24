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
        public Tree234(List<NodeViewModel> selNodes) : base(selNodes)
        {
            if (allNodesT234())
            {
                
                nodes = splitAllNodes(nodes);

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
                var temp = ((T234)(n.Node)).Split();
                if(temp.Count != 0)
                {
                    foreach (var t in temp)
                    {  
                        //create a viewmodel to each of the nodes
                        splitList.Add(new T234ViewModel(t));
                    }
                }
                //Add original T234viewmodel, now a twonode
                splitList.Add(n);

            }
            return splitList;
        }

        private T234ViewModel findMedian(List<T234ViewModel> treeList)
        {
            return treeList.ElementAt(treeList.Count/2);
        }

        public Tuple<List<LineViewModel>, List<NodeViewModel>> BalanceT234()
        {

            List<LineViewModel> lines = tAutoBalance();
            mergeNodes(lines, root);

            return new Tuple<List<LineViewModel>, List<NodeViewModel>>(lines,nodes);
        }
        private void mergeNodes(List<LineViewModel> lines, NodeViewModel node)
        {
            if (node != null)
            {
                var children = getChildren(node);
                ((T234)root.Node).Merge((T234)children[0].Node, (T234)children[1].Node);
                var leftgrandchildren = getChildren(children[0]);
                var rightgrandchildren = getChildren(children[1]);
                nodes.Remove(children[0]);
                nodes.Remove(children[1]);
                lines.Remove(lines.Find(l => l.From == node && l.To == children[0]));
                lines.Remove(lines.Find(l => l.From == node && l.To == children[1]));
                lines.Add(new LineViewModel(new Line { From = node.Node, To = leftgrandchildren[0].Node }));
                lines.Add(new LineViewModel(new Line { From = node.Node, To = leftgrandchildren[1].Node }));
                lines.Add(new LineViewModel(new Line { From = node.Node, To = rightgrandchildren[0].Node }));
                lines.Add(new LineViewModel(new Line { From = node.Node, To = rightgrandchildren[1].Node }));
                mergeNodes(lines, leftgrandchildren[0]);
                mergeNodes(lines, leftgrandchildren[1]);
                mergeNodes(lines, rightgrandchildren[0]);
                mergeNodes(lines, rightgrandchildren[1]);
            }
        }


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
