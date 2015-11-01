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

namespace AlgoTreeDraw.ViewModel
{
    public class BstViewModel : ViewModelBase
    {
        public ObservableCollection<Node> Nodes { get; set; }

        private Point initialMousePosition;
        private Point initialNodePosition;
        Boolean moved = true;

        public BstViewModel()
        {
            Nodes = new ObservableCollection<Node>() {
                new BST() { X = 0, Y = 0, diameter = 50},
                new BST() { X = 100, Y = 100, diameter = 50},
                new BST() { X = 100, Y = 200, diameter = 50}
            };

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
            mecs.MouseUpNode(e);
        }

        private void MouseMoveNode(MouseEventArgs e)
        {
            mecs.MouseMoveNode(e);
        }

        public void AddNode(Node e)
        {
            Node newNode = e.NewNode();
            Nodes.Add(newNode);
        }

        //Messages

        private object send(Node newNode)
        {
            var msg = new NodeMessage() { node = newNode };
            Messenger.Default.Send<NodeMessage>(msg);
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

    }
}
