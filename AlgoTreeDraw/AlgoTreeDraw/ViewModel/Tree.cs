using AlgoTreeDraw.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgoTreeDraw.ViewModel
{
    public class Tree : MainViewModelBase
    {
        public List<NodeViewModel> nodes;
        public NodeViewModel root;
        public List<NodeViewModel> hasBeenInsertedList = new List<NodeViewModel>();
        public List<int> subFromIndex = new List<int>();

        public List<LineViewModel> addedLinesAutoBalance = new List<LineViewModel>();

        // Temporary prolly
        //const int X_OFFSET = 70;
        //const int Y_OFFSET = 50;

        const int LEFT = 0;
        const int RIGHT = 1;
        const int X_OFFSET =  40;
        const int Y_OFFSET = 50;
        const int X_ONSET = 28;
        const int NOMOVE = -1;
        const int ONLY = 0;
        const int NONE = -1;

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
                    MessageBox.Show("Error: Some nodes does not have valid values (numbers)");

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
                    MessageBox.Show("Error: All nodes are not BST-nodes");
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
                    MessageBox.Show("Error: All nodes are not connected");
                    return false;
                }
            }
            return true;
        }

        public bool allNodesOneParentAndLessThanThreeChildren()
        {
            foreach(NodeViewModel n in nodes)
            {
                if (!hasOneParentAndLessThanThreeChildren(n) && !(n == root))
                {
                    //MessageBox?
                    MessageBox.Show("Error: Some nodes have more than one parent or too many children");

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

        public void insert(NodeViewModel newNode) 
        {
            if (selectedNodes.Count != 0)
            {
                nodes.Add(newNode);
                insertBST(newNode, root);
            }
            else MessageBox.Show("No node or tree selected");
            
        }

        public List<LineViewModel> tAutoBalance()
        {
            if (nodes != null && allNodesIntKeys() && allNodesBST() && allNodesConnected() && allNodesOneParentAndLessThanThreeChildren())
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
                return addedLinesAutoBalance;

            }
            return null;
            
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
            LineViewModel temp = new LineViewModel(new Line()) { From = n1, To = n2 };
            Lines.Add(temp);
            addedLinesAutoBalance.Add(temp);
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


        public bool makePretty()
        {
            if(nodes == null || nodes.Count <= 1)
            {
                //errrormsg
                return false;
            }else
            if(!allNodesConnected()){
                //errorMSG 
                return false;
            }



            List<NodeViewModel> bfList = getbfList(); //Updating the nodes in allNodes, to run the tree through breadth-first
           // NodeViewModel originalRoot = 
            double originalRootPos = bfList.ElementAt(0).X;
            //if (!isValidBST())
            //    return false;

            foreach (NodeViewModel nvm in bfList)
            {
                if (nvm.childrenFromList[0] == null) //IF THERE IS NO CHILDREN
                {

                }
                else if (nvm.childrenFromList[RIGHT] == null)    //One child
                {
                    if (nvm.childrenFromList[ONLY].isLeftChild)
                        nvm.moveOffset(nvm.childrenFromList[ONLY], LEFT);
                    else
                        nvm.moveOffset(nvm.childrenFromList[ONLY], RIGHT);
                    nvm.pushAncenstors(nvm.childrenFromList[ONLY]);
                }
                else
                {
                    nvm.moveOffset(nvm.childrenFromList[LEFT], LEFT);
                    nvm.moveOffset(nvm.childrenFromList[RIGHT], RIGHT);

                    nvm.pushAncenstors(nvm.childrenFromList[LEFT]);
                    nvm.pushAncenstors(nvm.childrenFromList[RIGHT]);
                }
            }
            //For making the root stationary :>
            if (originalRootPos < bfList.ElementAt(0).X)
                root.pushTree(LEFT, 64000, null, bfList.ElementAt(0).X - originalRootPos);
            else if (originalRootPos > bfList.ElementAt(0).X)
                root.pushTree(RIGHT, -64000, null, originalRootPos - bfList.ElementAt(0).X);

            return true;
        }

        private LinkedList<NodeViewModel> queue = new LinkedList<NodeViewModel>();
        public List<NodeViewModel> getbfList()
        {
            List<NodeViewModel> bfList = new List<NodeViewModel>();
            int i = 0;
            NodeViewModel nvm = root;
            queue.Clear();
            queue.AddLast(nvm);

            for (;;)
            {
                nvm.childrenFromList = nvm.getChildren();

                bfList.Add(nvm);
                i = 0;
                int selectedChildren = 0;
                foreach (NodeViewModel child in nvm.childrenFromList)
                    if (child != null)
                    {
                        if (nodes.Contains(child))
                        {
                            queue.AddLast(nvm.childrenFromList[i]);
                            selectedChildren++;
                        }
                        else
                            nvm.childrenFromList[i] = null;
                        i++;
                    }
                if (nvm.childrenFromList[LEFT] != null && selectedChildren == 1)
                {
                    nvm.childrenFromList[LEFT].isLeftChild = nvm.childrenFromList[LEFT].isSingleChildLeft();
                    Console.WriteLine("Do we get here???????????");
                }
                else if(i == 2 && selectedChildren == 2)
                {
                    nvm.childrenFromList[LEFT].isLeftChild = true;
                }
                else if(selectedChildren == 1 && i == 2)
                {
                    nvm.childrenFromList[LEFT] = nvm.childrenFromList[RIGHT];
                    nvm.childrenFromList[RIGHT] = null;
                }
                if (queue.Count == 1)
                    break;
                queue.RemoveFirst();
                nvm = queue.First();
            }
            return bfList;

        }

    }
}
