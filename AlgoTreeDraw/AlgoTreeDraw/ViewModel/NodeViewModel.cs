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
            set { Node.X = value; RaisePropertyChanged(); RaisePropertyChanged(() => CanvasCenterX);  if (value > 10) WIDTH = 1500; RaisePropertyChanged(() => WIDTH); Debug.Write(value + "\n" + "lol:" + WIDTH + "\n"); }
        }

        public int Offset { get; set; }  


        public double Y
        {
            get { return Node.Y; }
            set { Node.Y = value; RaisePropertyChanged(); RaisePropertyChanged(() => CanvasCenterY); if (value > HEIGHT) HEIGHT = (int)value; RaisePropertyChanged(() => HEIGHT); }
        }



        public double CanvasCenterX { get { return X + Diameter / 2; } set { X = value - Diameter / 2; RaisePropertyChanged(() => X); } }
        public double CanvasCenterY { get { return Y + Diameter / 2; } set { Y = value - Diameter / 2; RaisePropertyChanged(() => Y); } }

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
            set { Node.Key = value; RaisePropertyChanged(); }
        }

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
            set { borderThickness = value;  RaisePropertyChanged(); }
        }

        List<NodeViewModel> neighbours;

        // CONSTANTS
        const int LEFT = 0;
        const int RIGHT = 1;

        //returns true if tree is valid after add
        public void addNeighbour(NodeViewModel NVM)
        {
            // ViewModel
            neighbours.Add(NVM);
            NVM.neighbours.Add(this);

            // Model
            Node.neighbours.Add(NVM.Node);
            NVM.Node.neighbours.Add(Node);
        }

        //returns true if tree is valid after remove
        public void removeNeighbour(NodeViewModel NVM)
        {
            //ViewModel
            neighbours.Remove(NVM);
            NVM.neighbours.Remove(this);

            //Model
            Node.neighbours.Remove(NVM.Node);
            NVM.Node.neighbours.Remove(Node);
        }

        public bool isChild(NodeViewModel NVM)
        {
            return NVM.Y > Y;
        }

        //Returns true if node has maximum of one parent
        public bool isValid()
        {
            int count = 0;
            foreach (NodeViewModel neighbour in neighbours)
                if (neighbour.Y < Y) count++;

            return count <= 1 && neighbours.Count < 3;
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

        public bool isRoot()
        {
            foreach (NodeViewModel neighbour in neighbours)
                if (neighbour.Y < Y) return false;
            return true;
        }

        public NodeViewModel getRoot()
        {
            foreach (NodeViewModel neighbour in neighbours)
                if (neighbour.Y < Y) return neighbour.getRoot();
            return this;
        }

        public int childrenCount()
        {
            int children = 0;
            foreach (NodeViewModel neighbour in neighbours)
                if (neighbour.Y > Y) children++;
            return children;
        }

        public bool isValidBST()
        {
            // DER ER PROBLEM HER
            if (isValid() && childrenCount() <= 2)
            {
                NodeViewModel[] children = getChildren();
                foreach (NodeViewModel child in children)
                    if (child != null)
                        if (!child.isValidBST()) return false;
                if (children[RIGHT] != null) return int.Parse(children[LEFT].Key) <= int.Parse(children[RIGHT].Key);
            }
            else return false;
            return true;
        }

        //USE ON ROOT

        //USE ON ROOT //KIG MERE HER (visuelle del)
        //public bool autoAddBST(int _key)
        //{
        //    return Node.autoAddBST(_key);
        //}

        public Node getNode()
        {
            return Node;
        }

        public void makePretty()
        {
            Node.getRoot().makePretty();
        }

        public void autoBalance()
        {
            // if markedNodesConnected
            // and isValidBst
            // new list - sort all nodes in bst -> List<NodeViewModel> SortedList = nodesList.OrderBy(n=>n.Key).ToList();
            // eller objListOrder.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));
            // fjern alle linjer
            // Midterste element er rod - midterst til højre er højre barn ligeså med venstre osv osv. 
            // Brug insert i denne rækkefølge - sørg for at der også bliver tegnet linjer imellem dem
        }

        /*public bool isMarkedNodesConnected()
        {
            List<NodeViewModel> markedNodes = new List<NodeViewModel>();

        }

        public void markNodeAndNeighbours(NodeViewModel n, List<NodeViewModel> markedNodes)
        {
            if(!markedNodes.Contains(n))
            {
                markedNodes.Add(n);
                foreach
            }
        }*/

    }
}
