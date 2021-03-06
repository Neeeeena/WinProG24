﻿using AlgoTreeDraw.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgoTreeDraw.ViewModel
{
    public class Tree
    {
        ObservableCollection<LineViewModel> Lines;
        public List<NodeViewModel> nodes;
        public NodeViewModel root;
        public List<NodeViewModel> hasBeenInsertedList = new List<NodeViewModel>();
        public List<int> subFromIndex = new List<int>();
        public List<NodeViewModel> wholeTree = new List<NodeViewModel>();
        public List<LineViewModel> addedLinesAutoBalance = new List<LineViewModel>();

        const int LEFT = 0;
        const int RIGHT = 1;
        const int X_OFFSET =  40;
        const int Y_OFFSET = 50;
        const int X_ONSET = 28;
        const int NOMOVE = -1;
        const int ONLY = 0;
        const int NONE = -1;

        public Tree(List<NodeViewModel> selNodes, ObservableCollection<LineViewModel> lines)
        {
            // Tjek at selectedNodes ikke er 0 før new Tree(selectedNodes) bliver kaldt
            Lines = lines;
            if (!(selNodes.Count == 0))
            {
                nodes = selNodes;
                root = setRoot();
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
                if (!int.TryParse(n.TxtOne, out yolo))
                {
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
                    MessageBox.Show("Error: All nodes are not BST-nodes");
                    return false;
                }
            }
            return true;
        }

        public bool allNodesConnected()
        {
            unmarkNodes();
            root.hasBeenFound = true;
            markNeighbours(root);
            foreach(NodeViewModel n in nodes)
            {
                if (!n.hasBeenFound)
                {
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
                if ((!hasOneParentAndLessThanThreeChildren(n) && !(n == root)) || (n==root && n.neighbours.Count() > 2))
                {
                    MessageBox.Show("Error: Some nodes have more than one parent or too many children");

                    return false;
                }
            }
            return true;
        }

        public void markNeighbours(NodeViewModel nvm)
        {
            foreach(NodeViewModel n in nvm.neighbours)
            {
                if(!n.hasBeenFound && nodes.Contains(n))
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

        public bool checkChildrenKey(NodeViewModel nvm)
        {
            NodeViewModel[] children = getChildren(nvm);
            foreach (NodeViewModel child in children) {
                if (child != null) { 
                    if (!checkChildrenKey(child)) return false;
                }
            }
            if (!checkCorrectParentToRoot(nvm)) return false;
            if (children[0] == null) return true;
            if (children[1] == null)
            {
                if (int.Parse(children[0].TxtOne) <= int.Parse(nvm.TxtOne) && children[0].X < nvm.X) return true;
                if (int.Parse(children[0].TxtOne) > int.Parse(nvm.TxtOne) && children[0].X > nvm.X) return true;
                return false;
            }
            // getChildren already switches such that children[0] is further to the left than children[1]. 
            // Therefore it is enough to check that children[0].TxtOne <= nvm.TxtOne and not also >
            return (int.Parse(children[0].TxtOne) <= int.Parse(nvm.TxtOne) && int.Parse(children[1].TxtOne) > int.Parse(nvm.TxtOne) &&
                children[0].X < nvm.X && children[1].X > nvm.X);
        }

        public bool checkCorrectParentToRoot(NodeViewModel nvm)
        {
            NodeViewModel grandParent = nvm.getParent();
            NodeViewModel parent = nvm.getParent();
        
            while (grandParent != root)
            {
                if (grandParent == null || grandParent.getParent() == null) break;
                grandParent = grandParent.getParent();                
                if (parent.X < grandParent.X && int.Parse(nvm.TxtOne) > int.Parse(grandParent.TxtOne) ||
                    parent.X > grandParent.X && int.Parse(nvm.TxtOne) <= int.Parse(grandParent.TxtOne))
                    return false;
                parent = parent.getParent();
            }
            return true;            
        }

        public bool childrenKeyCorrectlyPlaced()
        {
            if(checkChildrenKey(root)) return true;
            MessageBox.Show("The chosen tree is not a valid BST" +
                "\nMake sure that every node to the left is smaller than it's parent and every node to the right is larger" +
                "\nAlso make sure that left child is further to the left than it's parent and the same with the right");
            return false;
        }

        public void insert(NodeViewModel newNode) 
        {
            insertBST(newNode, root);      
        }

        public bool isValidBST()
        {
            return hasIntKeysAndBSTNodes() && allNodesConnected() && allNodesOneParentAndLessThanThreeChildren() && childrenKeyCorrectlyPlaced();
        }
        

        public List<NodeViewModel> getWholeTree()
        {
            wholeTree.Add(root);
            generateTreeFromOneNode(root);
            return wholeTree;
        }

        public void generateTreeFromOneNode(NodeViewModel nvm)
        {
            foreach(NodeViewModel n in nvm.neighbours)
            {
                if(!wholeTree.Contains(n))
                {
                    wholeTree.Add(n);
                    generateTreeFromOneNode(n);
                }
            }         
        }

        public List<LineViewModel> tAutoBalance()
        {
            nodes.Sort((x, y) => int.Parse(x.TxtOne).CompareTo(int.Parse(y.TxtOne)));
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
            makePretty(false);
            return addedLinesAutoBalance;
    
        }

        public bool hasIntKeysAndBSTNodes()
        {
            return (allNodesBST() && allNodesIntKeys());
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

            //Zero children
            if (children[0] == null)
            {
                addNeighbourAndLineAndUpdatePosition(newNode, nvm);
            }

            // One child
            else if (children[1] == null)
            {
                // If both the new node and the one child needs to be on the same side
                if ((int.Parse(newNode.TxtOne) <= int.Parse(nvm.TxtOne) && int.Parse(children[0].TxtOne) <= int.Parse(nvm.TxtOne)) ||
                    (int.Parse(newNode.TxtOne) > int.Parse(nvm.TxtOne) && int.Parse(children[0].TxtOne) > int.Parse(nvm.TxtOne)))
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
                if (int.Parse(newNode.TxtOne) <= int.Parse(nvm.TxtOne))
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
            if(int.Parse(n1.TxtOne) > int.Parse(n2.TxtOne))
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
            if (nvm != null)
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
            return null;
        }

        public void removeLinesAndNeighbours()
        {
            List<LineViewModel> tempLines = new List<LineViewModel>();
            List<NodeViewModel> tempNeighs = new List<NodeViewModel>();
            foreach (NodeViewModel n in nodes)
            {
                foreach(LineViewModel l in Lines)
                {
                    if ((l.From == n || l.To == n) && !tempLines.Contains(l)) tempLines.Add(l);
                }
                foreach (NodeViewModel neigh in n.neighbours)
                {
                    tempNeighs.Add(neigh);
                }
                foreach (NodeViewModel neigh in tempNeighs)
                {
                    n.removeNeighbour(neigh);
                }
                tempNeighs.Clear();
            }
            
            foreach(LineViewModel l in tempLines)
            {
                Lines.Remove(l);
            }
        }


        public bool makePretty(bool entireTree)
        {
            if(nodes == null || nodes.Count <= 1)
            {
                //errrormsg
                return false;
            }else if(!allNodesConnected()){
                //errorMSG 
                return false;
            }
            foreach(NodeViewModel n in nodes)
            {
                if (n is T234ViewModel)
                {
                    MessageBox.Show("2-3-4 tree align not implemented");
                    return false;
                }
            }

            List<NodeViewModel> bfList = getbfList(entireTree); //Updating the nodes in allNodes, to run the tree through breadth-first
            double originalRootPos = bfList.ElementAt(0).X;
            
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


            foreach(NodeViewModel n in nodes)
            {
                if(n.X < 10)
                {
                    root.pushTree(RIGHT, -64000, null, -n.X+10);
                }
            }
            return true;
        }

        private LinkedList<NodeViewModel> queue = new LinkedList<NodeViewModel>();
        public List<NodeViewModel> getbfList(bool entireTree)
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
                            if(!entireTree)
                                nvm.childrenFromList[i] = null;
                        i++;
                    }
                if ((nvm.childrenFromList[LEFT] != null && selectedChildren == 1) || entireTree && i == 1)
                {
                    nvm.childrenFromList[LEFT].isLeftChild = nvm.childrenFromList[LEFT].isSingleChildLeft();
                }
                else if(i == 2 && selectedChildren == 2 && !entireTree)
                {
                    nvm.childrenFromList[LEFT].isLeftChild = true;
                }
                else if(selectedChildren == 1 && i == 2 && !entireTree)
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


        public NodeViewModel remove(NodeViewModel _nvm)
        {
            return removeBST(_nvm);
        }

        public NodeViewModel removeBST(NodeViewModel _nvm)
        {
            NodeViewModel removeNode = null;
            removeNode = _nvm;

            NodeViewModel parent = removeNode.getParent();
            NodeViewModel[] children = removeNode.getChildren();
            NodeViewModel replacementNode;
            if (children[LEFT] == null)
            {
                return removeNode;
            }
            else
            {
                replacementNode = children[LEFT].getMostRightNode();
                for(;;)
                {
                    children = replacementNode.getChildren();
                    removeNode.TxtOne = replacementNode.TxtOne;
                    removeNode.Color = replacementNode.Color;
                    removeNode.ColorOfText = replacementNode.ColorOfText;
                    removeNode.PreColor = replacementNode.PreColor;

                    if (children[LEFT] == null)
                        return replacementNode;
                    else
                    {
                        removeNode = replacementNode;
                        replacementNode = children[LEFT].getMostRightNode();
                    }
                }           
            }
        }

    }
}
