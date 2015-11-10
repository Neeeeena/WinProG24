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

        private Visibility isEditing = Visibility.Hidden;
        private Visibility isNotEditing = Visibility.Visible;
        public Visibility IsEditing { get { return isEditing; } set { isEditing = value; RaisePropertyChanged(); } }
        public Visibility IsNotEditing { get { return isNotEditing; } set { isNotEditing = value; RaisePropertyChanged(); } }

        public NodeViewModel(Node node)
        {
            _node = node;

        }

        //View databinds to the following
        Node _node;

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
            set { Node.X = value; RaisePropertyChanged(); RaisePropertyChanged(() => CanvasCenterX); }
        }



        public double Y
        {
            get { return Node.Y; }
            set { Node.Y = value; RaisePropertyChanged(); RaisePropertyChanged(() => CanvasCenterY); }
        }



        public double CanvasCenterX { get { return X + Diameter / 2; } set { X = value - Diameter / 2; RaisePropertyChanged(() => X); } }
        public double CanvasCenterY { get { return Y + Diameter / 2; } set { Y = value - Diameter / 2; RaisePropertyChanged(() => Y); } }

        private bool isMoveSelected;
        public bool IsMoveSelected { get { return isMoveSelected; } set { isMoveSelected = value; RaisePropertyChanged(); } }

        public double Diameter
        {
            get { return Node.diameter; }
            set { Node.diameter = value; }
        }

        public string VisualText
        {
            get { return Node.VisualText; }
            set { Node.VisualText = value; RaisePropertyChanged(); }
        }

        public Brush Color
        {
            get { return Node.color; }
            set { Node.color = value; RaisePropertyChanged(); }
        }

        public Brush PreColor
        {
            get { return Node.preColor; }
            set { Node.preColor = value; RaisePropertyChanged(); }
        }




        public LinkedList<Node> neighbours = new LinkedList<Node>();


        //returns true if tree is valid after add
        public bool addNeighbour(Node node)
        {
            return Node.addNeighbour(node);
        }

        //returns true if tree is valid after remove
        public bool removeNeighbour(Node node)
        {
            return Node.removeNeighbour(node);
        }

        public bool isChild(Node node)
        {
            return Node.isChild(node);
        }

        //Returns true if node has maximum of one parent
        public bool isValid()
        {
            return Node.isValid();
        }



        public bool isRoot()
        {
            return Node.isRoot();
        }

        public Node getRoot()
        {
            return Node.getRoot();
        }

        public int childrenCount()
        {
            return Node.childrenCount();
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

    }
}
