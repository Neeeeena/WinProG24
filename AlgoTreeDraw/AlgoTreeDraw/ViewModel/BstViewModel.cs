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

namespace AlgoTreeDraw.ViewModel
{
    public class BstViewModel : ViewModelBase
    {
        public ObservableCollection<Node> Nodes { get; set; }

        public bool isAddingLine = false;
        public Node LineFrom;
        public BstViewModel()
        {
            Nodes = new ObservableCollection<Node>() {
                new BST() { X = 0, Y = 0, diameter = 50},
                new BST() { X = 100, Y = 100, diameter = 50},
                new BST() { X = 100, Y = 200, diameter = 50}
            };
            Messenger.Default.Register<NodeMessage>(this, (action) => ReceiveMessage(action));
            Messenger.Default.Register<LineMessage>(this, (action) => AddLineMSG(action));

            MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);

            mecs = new MouseEventCommands();

        }

        //Commands

        public ICommand MouseDownNodeCommand { get; }
        public ICommand MouseMoveNodeCommand { get; }
        public ICommand MouseUpNodeCommand { get; }

        private MouseEventCommands mecs { get; set; }

        private void MouseDownNode(MouseButtonEventArgs e)
        {
            mecs.MouseDownNode(e);
        }

        private void MouseUpNode(MouseButtonEventArgs e)
        {
            Node node = mecs.MouseUpNode(e);

            if (isAddingLine)
            {
                if (LineFrom == null) { LineFrom = node; LineFrom.Color = Brushes.Blue; }
                else if(LineFrom != null && !Object.ReferenceEquals(LineFrom, node)) { AddLine(node); }
              
            }

        }

        private void MouseMoveNode(MouseEventArgs e)
        {
            mecs.MouseMoveNode(e);
        }

        public void AddLine(Node node)
        {
            Messenger.Default.Send<LineMessage>(new LineMessage() { line = new Line() { From = LineFrom , To = node }, isAddline = false } );
            //LineFrom.Color = Brushes.White;
            LineFrom = null;
            isAddingLine = false;
        }

        public void AddNode(Node node)
        {
            Node newNode = new BST() { X = node.X, Y = node.Y, diameter = 50};
            Nodes.Add(newNode);
        }

        //Messages

        private object send(Node newNode)
        {
            var msg = new NodeMessage() { node = newNode };
            //Messenger.Default.Send<NodeMessage>(msg);
            return null;
        }

        private object AddLineMSG(LineMessage action)
        {
            isAddingLine = action.isAddline;
            if(isAddingLine == false)
            {
                if(LineFrom != null) LineFrom.Color = Brushes.White;
                LineFrom = null;
            }
            return null;
        }

        private object ReceiveMessage(NodeMessage action)
        {
            AddNode(action.node);
            return null;
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

        public double X
        {
            get { return Node.X; }
            set { Node.X = value; }
        }
        public double Y
        {
            get { return Node.Y; }
            set { Node.Y = value; }
        }

        public Brush Color
        {
            get { return Node.Color; }
            set { Node.Color = value; }
        }

    }
}
