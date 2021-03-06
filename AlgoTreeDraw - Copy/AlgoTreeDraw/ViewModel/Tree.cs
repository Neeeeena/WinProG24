﻿using AlgoTreeDraw.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.ViewModel
{
    public class Tree : MainViewModelBase
    {
        public List<NodeViewModel> nodes;
        public NodeViewModel root;
        public List<NodeViewModel> hasBeenInsertedList = new List<NodeViewModel>();
        public List<int> subFromIndex = new List<int>();

        // Temporary prolly
        const int X_OFFSET = 70;
        const int Y_OFFSET = 50;

        public Tree(List<NodeViewModel> selNodes)
        {
            // Tjek at selectedNodes ikke er 0 før new Tree(selectedNodes) bliver kaldt
            if (!(selNodes.Count == 0))
            {
                nodes = selNodes;
                unmarkNodes();
                root = setRoot();

                // Sort test works
                //foreach (NodeViewModel n in nodes)
                //{
                //    Console.WriteLine(n.Key);
                //}
                //Console.WriteLine("Oh yea");
                //nodes.Sort((x, y) => int.Parse(x.Key).CompareTo(int.Parse(y.Key)));
                //foreach (NodeViewModel n in nodes)
                //{
                //    Console.WriteLine(n.Key);
                //}
            }
        }

        public NodeViewModel setRoot()
        {
            NodeViewModel returnedNode = nodes.ElementAt(0);
            foreach(NodeViewModel n in nodes)
            {
                if (n.Y < returnedNode.Y) returnedNode = n;
            }
            return returnedNode;
        }

        public void unmarkNodes()
        {
            foreach(NodeViewModel n in nodes)
            {
                n.hasBeenFound = false;
            }
        }

        public bool allNodesIntKeys()
        {
            int yolo;
            foreach(NodeViewModel n in nodes)
            {
                if (!int.TryParse(n.Key, out yolo))
                {
                    Console.WriteLine("All Nodes not valid keys");
                    return false;
                }
            }
            return true;
        }

        public bool allNodesBST()
        {
            foreach(NodeViewModel n in nodes)
            {
                if (!(n is BSTViewModel))
                {
                    //Messagebox?
                    Console.WriteLine("All Nodes not bst");
                    return false;
                }
            }
            return true;
        }

        public bool allNodesConnected()
        {
            root.hasBeenFound = true;
            markNeighbours(root);
            foreach(NodeViewModel n in nodes)
            {
                if (!n.hasBeenFound)
                {
                    //MessageBox?
                    Console.WriteLine("All nodes not connected");
                    return false;
                }
            }
            return true;
        }

        public bool allNodesOneParentAndLessThanTwoChildren()
        {
            foreach(NodeViewModel n in nodes)
            {
                if (!hasOneParentAndLessThanThreeChildren(n) && !(n == root))
                {
                    //MessageBox?
                    Console.WriteLine("All Nodes does not have one parent and less than two children");
                    Console.WriteLine(n.Key);
                    Console.WriteLine(root.Key);
                    foreach(NodeViewModel bla in n.neighbours)
                    {
                        Console.WriteLine("Jeg skriver ikke noget kap");
                    }
                    return false;
                }
            }
            return true;
        }

        public void markNeighbours(NodeViewModel nvm)
        {
            foreach(NodeViewModel n in nvm.neighbours)
            {
                if(!n.hasBeenFound)
                {
                    n.hasBeenFound = true;
                    markNeighbours(n);
                }
            }
        }

        public bool hasOneParentAndLessThanThreeChildren(NodeViewModel nvm)
        {
            int pCount = 0;
            foreach (NodeViewModel n in nvm.neighbours)
            {
                if (n.Y < nvm.Y) pCount++;
            }
            return pCount == 1 && nvm.neighbours.Count <= 3;
        }

        public void tAutoBalance()
        {
            if (nodes != null && allNodesIntKeys() && allNodesBST() && allNodesConnected() && allNodesOneParentAndLessThanTwoChildren())
            {
                nodes.Sort((x, y) => int.Parse(x.Key).CompareTo(int.Parse(y.Key)));
                removeLinesAndNeighbours();
                root = nodes.ElementAt(nodes.Count / 2);
                hasBeenInsertedList.Add(root);
                subFromIndex.Add((nodes.Count / 2 + 1) / 2);
                balancedInsert(nodes.Count / 2,0);

                // Inserts the rest of the nodes (they will all be leaves)
                foreach (NodeViewModel n in nodes)
                {
                    if (!hasBeenInsertedList.Contains(n))
                    {
                        insertBST(n, root);
                    }
                }

            }
            
            // if markedNodesConnected
            // and isValidBst
            // new list - sort all nodes in bst -> List<NodeViewModel> SortedList = nodesList.OrderBy(n=>n.Key).ToList();
            // eller objListOrder.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));
            // fjern alle linjer
            // Midterste element er rod - midterst til højre er højre barn ligeså med venstre osv osv. 
            // Brug insert i denne rækkefølge - sørg for at der også bliver tegnet linjer imellem dem
        }


        private void balancedInsert(int index, int subIndex)
        {
            // If less than 3, no autobalance is needed
            if (nodes.Count > 2)
            {
                
                // If sub is 1, then all the remaining nodes will be leaves and can just be inserted
                // It has been ran log(n)-1 times which should be the maximum depth without leaves
                if (subFromIndex.ElementAt(subIndex) != 1)
                {
                    // Remebers which nodes has been inserted from the nodes-list
                    hasBeenInsertedList.Add(nodes.ElementAt(index - subFromIndex.ElementAt(subIndex)));
                    hasBeenInsertedList.Add(nodes.ElementAt(index + subFromIndex.ElementAt(subIndex)));

                    // Inserts the nodes from the root
                    insertBST(nodes.ElementAt(index - subFromIndex.ElementAt(subIndex)), root);
                    insertBST(nodes.ElementAt(index + subFromIndex.ElementAt(subIndex)), root);

                    if (subIndex == subFromIndex.Count - 1) subFromIndex.Add((index - subFromIndex.ElementAt(subIndex) + 1) / 2);


                    balancedInsert(index - subFromIndex.ElementAt(subIndex), subIndex + 1);
                    balancedInsert(index + subFromIndex.ElementAt(subIndex), subIndex + 1);
                }
            }
        }

        // Antaget at træet er korrekt, men virker muligvis stadig alligevel
        public void insertBST(NodeViewModel newNode, NodeViewModel nvm)
        {
            NodeViewModel[] children = getChildren(nvm);
            // Zero children
            if (children[0] == null)
            {
                addNeighbourAndLineAndUpdatePosition(newNode, nvm);                 
            }
            // One child
            else if(children[1] == null)
            {
                // If both the new node and the one child needs to be on the same side
                if ((int.Parse(newNode.Key) <= int.Parse(nvm.Key) && int.Parse(children[0].Key) <= int.Parse(nvm.Key)) ||
                    (int.Parse(newNode.Key) > int.Parse(nvm.Key) && int.Parse(children[0].Key) > int.Parse(nvm.Key)))
                {
                    insertBST(newNode, children[0]);
                }
                else
                {
                    addNeighbourAndLineAndUpdatePosition(newNode, nvm);
                }
            }
            // Two children
            else
            {
                if(int.Parse(newNode.Key) < int.Parse(nvm.Key))
                {
                    insertBST(newNode, children[0]);
                }
                else
                {
                    insertBST(newNode, children[1]);
                }
            }
        }

        public void addNeighbourAndLineAndUpdatePosition(NodeViewModel n1, NodeViewModel n2)
        {
            n1.addNeighbour(n2);
            Lines.Add(new LineViewModel(new Line()) { From = n1, To = n2 });
            if(int.Parse(n1.Key) > int.Parse(n2.Key))
            {
                n1.X = n2.X + X_OFFSET;
                n1.Y = n2.Y + Y_OFFSET;
            }
            else
            {
                n1.X = n2.X - X_OFFSET;
                n1.Y = n2.Y + Y_OFFSET;
            }
            
        }

        public NodeViewModel[] getChildren(NodeViewModel nvm)
        {

            NodeViewModel[] children = new NodeViewModel[2];
            int i = 0;
            foreach (NodeViewModel n in nvm.neighbours)
            {
                if (n.Y > nvm.Y)
                {
                    children[i] = n;
                    i++;
                }
            }                

            if (i == 2 && children[0].X > children[1].X)
            {
                NodeViewModel temp = children[0];
                children[0] = children[1];
                children[1] = temp;
            }
            return children;
        }

        // FUCKING TEMP TING PGA ILLIGEAL OPERATION REMOVE LIST MAN FOREACHER GOD DAMMIT BEDRE LØSNING PLz!!?
        public void removeLinesAndNeighbours()
        {
            List<LineViewModel> tempLines = new List<LineViewModel>();
            List<NodeViewModel> tempNeighs = new List<NodeViewModel>();
            foreach (NodeViewModel n in nodes)
            {
                foreach(LineViewModel l in Lines)
                {
                    if ((l.From == n || l.To == n)) tempLines.Add(l);
                }
                foreach (NodeViewModel neigh in n.neighbours)
                {
                    tempNeighs.Add(neigh);
                }
                foreach (NodeViewModel neigh in tempNeighs)
                {
                    n.removeNeighbour(neigh);
                }
            }
            
            foreach(LineViewModel l in tempLines)
            {
                Lines.Remove(l);
            }
        }

    }
}
