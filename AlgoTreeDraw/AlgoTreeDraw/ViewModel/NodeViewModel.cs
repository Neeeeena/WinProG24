using AlgoTreeDraw.Model;
using System;
using GalaSoft.MvvmLight;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.CommandWpf;
using AlgoTreeDraw.Command;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;
using System.Diagnostics;

namespace AlgoTreeDraw.ViewModel
{
    public abstract class NodeViewModel : MainViewModelBase
    {
        public ICommand DeleteCommand { get; }

        private Visibility isEditing = Visibility.Hidden;
        private Visibility isNotEditing = Visibility.Visible;
        public Visibility IsEditing { get { return isEditing; } set { isEditing = value; RaisePropertyChanged(); } }
        public Visibility IsNotEditing { get { return isNotEditing; } set { isNotEditing = value; RaisePropertyChanged(); } }

        // Select
        public Point initialNodePosition = new Point();
        private Brush color;
        private Brush preColor;


        public Brush Color
        {
            get { return color; }
            set { color = value; setNodeColor(value); RaisePropertyChanged(); }
        }

        public Brush PreColor
        {
            get { return preColor; }
            set { preColor = value; setNodePreColor(value); RaisePropertyChanged(); }
        }

        private Brush borderColor;

        public Brush BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; RaisePropertyChanged(); }
        }

        private double borderThickness;

        public NodeViewModel(Node node)
        {
            color = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, node.color.R, node.color.G, node.color.B));
            preColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, node.preColor.R, node.preColor.G, node.preColor.B));
            _node = node;
            Offset = 47;
            DeleteCommand = new RelayCommand<MouseButtonEventArgs>(deleteNode);
            borderColor = Brushes.Black;
            borderThickness = 1;


        }

        //View databinds to the following
        Node _node;

        public void deleteNode(MouseButtonEventArgs e)
        {
            Console.WriteLine("Delete Kaldt");
            if (selectedNodes.Contains(this))
            {
                undoRedo.InsertInUndoRedo(new DeleteNodeCommand(Nodes, selectedNodes, Lines));
            }
            else
            {
                clearSelectedNodes();
                selectedNodes.Add(this);
                undoRedo.InsertInUndoRedo(new DeleteNodeCommand(Nodes, selectedNodes, Lines));
                selectedNodes.Clear();
            }
        }

        public Node Node
        {
            get
            {
                return _node;
            }
            set
            {
                _node = value;
            }
        }

        public abstract NodeViewModel newNodeViewModel();

        public double X
        {
            get { return Node.X; }
            set { Node.X = value; RaisePropertyChanged(); RaisePropertyChanged(() => CanvasCenterX); if (value > 10) WIDTH = 1500; RaisePropertyChanged(() => WIDTH);}
        }

        public int Offset { get; set; }


        public double Y
        {
            get { return Node.Y; }
            set { Node.Y = value; RaisePropertyChanged(); RaisePropertyChanged(() => CanvasCenterY); if (value > HEIGHT) HEIGHT = (int)value; RaisePropertyChanged(() => HEIGHT); }
        }

        public int ID
        {
            get { return Node.ID; }
            set { Node.ID = value; RaisePropertyChanged(); }
        }

        public double CanvasCenterX { get { return X + Diameter / 2; } set { X = value - Diameter / 2; RaisePropertyChanged(() => X); } }
        public double CanvasCenterY { get { return Y + Diameter / 2; } set { Y = value - Diameter / 2; RaisePropertyChanged(() => Y); } }

        // Unused
        private bool isSelected;
        public bool IsSelected { get { return isSelected; } set { isSelected = value; RaisePropertyChanged(); } }

        public Brush _ColorOfText;

        public Brush ColorOfText { get { return _ColorOfText; } set { _ColorOfText = value; RaisePropertyChanged(); } }

        public Brush _PreColorOfText = Brushes.Black;
        public Brush PreColorOfText { get { return _PreColorOfText; } set { _PreColorOfText = value; RaisePropertyChanged(); } }

        public double Diameter
        {
            get { return Node.diameter; }
            set { Node.diameter = value; }
        }

        public string Key
        {
            get { return Node.Key; }
            set { Node.PreKey = Node.Key; Node.Key=value; RaisePropertyChanged(); }
        }
        public string PreKey
        {
            get { return Node.PreKey; }
            set { Node.PreKey = value; }
        }

        public bool hasBeenFound { get; set; }

        public void setNodeColor(Brush _color)
        {
            byte r = ((Color)_color.GetValue(SolidColorBrush.ColorProperty)).R;
            byte b = ((Color)_color.GetValue(SolidColorBrush.ColorProperty)).B;
            byte g = ((Color)_color.GetValue(SolidColorBrush.ColorProperty)).G;
            Node.color.R = r;
            Node.color.B = b;
            Node.color.G = g;
        }

        public void setNodePreColor(Brush _color)
        {
            byte r = ((Color)_color.GetValue(SolidColorBrush.ColorProperty)).R;
            byte b = ((Color)_color.GetValue(SolidColorBrush.ColorProperty)).B;
            byte g = ((Color)_color.GetValue(SolidColorBrush.ColorProperty)).G;
            Node.preColor.R = r;
            Node.preColor.B = b;
            Node.preColor.G = g;
        }


        public double BorderThickness
        {
            get { return borderThickness; }
            set { borderThickness = value; RaisePropertyChanged(); }
        }


        // CONSTANTS
        const int LEFT = 0;
        const int RIGHT = 1;
        const int X_OFFSET = 40;
        const int Y_OFFSET = 50;
        const int X_ONSET = 28;
        const int NOMOVE = -1;
        const int ONLY = 0;
        const int NONE = -1;

        

        public List<NodeViewModel> neighbours = new List<NodeViewModel>();

        private LinkedList<NodeViewModel> queue = new LinkedList<NodeViewModel>();
        public bool isLeftChild = false;
        //public bool IsLeftChild { get { return isLeftChild; } }
        public NodeViewModel[] childrenFromList;
        //public NodeViewModel[] ChildrenFromList { get { return childrenFromList; } }



        public void addNeighbour(NodeViewModel NVM)
        {
            // ViewModel
            neighbours.Add(NVM);
            NVM.neighbours.Add(this);
        }

        //returns true if tree is valid after remove
        public void removeNeighbour(NodeViewModel NVM)
        {
            //ViewModel
            neighbours.Remove(NVM);
            NVM.neighbours.Remove(this);
        }

        public bool isChild(NodeViewModel NVM)
        {
            return NVM.Y > Y;
        }

        public NodeViewModel[] getChildren()
        {

            NodeViewModel[] children = new NodeViewModel[3];
            int i = 0;
            foreach (NodeViewModel neighbour in neighbours)
                if (neighbour.Y > Y)
                {
                    children[i] = neighbour;
                    i++;
                }

            if (i == 2 && children[0].X > children[1].X)
            {
                NodeViewModel temp = children[0];
                children[0] = children[1];
                children[1] = temp;
            }
            return children;
        }

        //public bool isRoot()
        //{
        //    foreach (NodeViewModel neighbour in neighbours)
        //        if (neighbour.Y < Y) return false;
        //    return true;
        //}

        public NodeViewModel getRoot()
        {
            foreach (NodeViewModel neighbour in neighbours)
                if (neighbour.Y < Y) return neighbour.getRoot();
            return this;
        }

        //MÅSKE SKROTTES
        public int childrenCount()
        {
            int children = 0;
            foreach (NodeViewModel neighbour in neighbours)
                if (neighbour.Y > Y) children++;
            return children;
        }

        //public bool isValidBST()
        //{
        //    // DER ER PROBLEM HER
        //    if (isValid() && childrenCount() <= 2)
        //    {
        //        NodeViewModel[] children = getChildren();
        //        foreach (NodeViewModel child in children)
        //            if (child != null)
        //                if (!child.isValidBST()) return false;
        //        if (children[RIGHT] != null) return int.Parse(children[LEFT].Key) <= int.Parse(children[RIGHT].Key);
        //    }
        //    else return false;
        //    return true;
        //}

        ////USE ON ROOT

        ////USE ON ROOT //KIG MERE HER (visuelle del)
        ////public bool autoAddBST(int _key)
        ////{
        ////    return Node.autoAddBST(_key);
        ////}


        
        /*public bool isMarkedNodesConnected()
        {

            List<NodeViewModel> markedNodes = new List<NodeViewModel>();
            //Her kan getbfList anvendes (tidligere updateList)
        }

        public void markNodeAndNeighbours(NodeViewModel n, List<NodeViewModel> markedNodes)
        {
            if(!markedNodes.Contains(n))
            {
                markedNodes.Add(n);
                foreach
            }
        }*/

        public bool isSingleChildLeft()
        {
            NodeViewModel parent = getParent();
            if (parent == null)
                return false;
            else if (parent.X < X)
                return false;
            else
                return true;
        }

        public NodeViewModel getParent()
        {
            foreach (NodeViewModel neighbour in neighbours)
                if (neighbour.Y < Y) return neighbour;
            return null;
        }

        public NodeViewModel getMostRightNode()
        {
            NodeViewModel[] children = getChildren();
            if(children[RIGHT] != null)
            {
                return children[RIGHT].getMostRightNode();
            }
            return this;
        }

        //public bool makePretty()
        //{
        //    List<NodeViewModel> bfList = getbfList(); //Updating the nodes in allNodes, to run the tree through breadth-first
        //    double originalRootPos = bfList.ElementAt(0).X;
        //    //if (!isValidBST())
        //    //    return false;

        //    foreach (NodeViewModel nvm in bfList)
        //    {
        //        if (nvm.childrenFromList[0] == null) //IF THERE IS NO CHILDREN
        //        {

        //        }
        //        else if (nvm.childrenFromList[RIGHT] == null)    //One child
        //        {
        //            if (nvm.childrenFromList[ONLY].isLeftChild)
        //                nvm.moveOffset(nvm.childrenFromList[ONLY], LEFT);
        //            else
        //                nvm.moveOffset(nvm.childrenFromList[ONLY], RIGHT);
        //            nvm.pushAncenstors(nvm.childrenFromList[ONLY]);
        //        }
        //        else
        //        {
        //            nvm.moveOffset(nvm.childrenFromList[LEFT], LEFT);
        //            nvm.moveOffset(nvm.childrenFromList[RIGHT], RIGHT);

        //            nvm.pushAncenstors(nvm.childrenFromList[LEFT]);
        //            nvm.pushAncenstors(nvm.childrenFromList[RIGHT]);
        //        }
        //    }
        //    //For making the root stationary :>
        //    if (originalRootPos < bfList.ElementAt(0).X)
        //        pushTree(LEFT, 64000, null, bfList.ElementAt(0).X - originalRootPos);
        //    else if (originalRootPos > bfList.ElementAt(0).X)
        //        pushTree(RIGHT, -64000, null, originalRootPos - bfList.ElementAt(0).X);

        //    return true;
        //}

        public bool pushAncenstors(NodeViewModel orig)
        {
            NodeViewModel parent = getParent();
            if (parent == null)
            {
                return false;
            }

            if (parent.X + X_ONSET - 1 > orig.X && parent.X - X_ONSET + 1 < orig.X)
            {
                if (X < parent.X)
                {
                    getRoot().pushTree(LEFT, parent.X, orig, X_ONSET + (orig.X - parent.X));
                    orig.move(LEFT, X_ONSET + (orig.X - parent.X));
                }
                else
                {
                    getRoot().pushTree(RIGHT, parent.X, orig, X_ONSET + (parent.X - orig.X));
                    orig.move(RIGHT, X_ONSET + (parent.X - orig.X));
                }

            }
            return parent.pushAncenstors(orig);
        }

        public bool pushTree(int direction, double threshold, NodeViewModel orig, double offset)
        {
            NodeViewModel[] children = getChildren();
            if (this == orig)
                return true;
            if (direction == LEFT && X < threshold)
                move(LEFT, offset);
            else
            if (direction == RIGHT && threshold < X)
                move(RIGHT, offset);

            int i = 0;
            //while (children[i] != null)
            foreach(NodeViewModel child in children)
                if(child != null)
                {
                    children[i].pushTree(direction, threshold, orig, offset);
                    i++;
                }
            return true;
        }

        public bool move(int direction, double offset)
        {
            if (LEFT == direction)
                X = X - offset;
            else
                X = X + offset;
            return true;
        }

        public bool moveOffset(NodeViewModel child, int direction)
        {
            if (LEFT == direction)
                child.X = X - X_OFFSET;
            else
                child.X = X + X_OFFSET;
            child.Y = Y + Y_OFFSET;
            return true;
        }

       // private List<NodeViewModel> bfList;
        //public List<NodeViewModel> getbfList(List<NodeViewModel> markedArea)
        //{
        //    List<NodeViewModel> bfList = new List<NodeViewModel>();
        //    int i = 0;
        //    NodeViewModel nvm = markedArea.root;
        //    queue.Clear();
        //    queue.AddLast(nvm);

        //    for (;;)
        //    {
        //        if (markedArea.Contains(nvm)) {
        //            Console.WriteLine("Does the code even get here?");
        //            nvm.childrenFromList = nvm.getChildren();
        //            bfList.Add(nvm);
        //            i = 0;
        //            foreach (NodeViewModel child in nvm.childrenFromList)
        //                if (child != null)
        //                {
        //                    queue.AddLast(nvm.childrenFromList[i]);
        //                    i++;
        //                }
        //            if (i == 1)
        //                nvm.childrenFromList[LEFT].isLeftChild = nvm.childrenFromList[LEFT].isSingleChildLeft();

                    

        //        }
        //        if (queue.Count == 1)
        //            break;
        //        queue.RemoveFirst();
        //        nvm = queue.First();
        //    }
        //    return bfList;

        //}
        

    }
}
