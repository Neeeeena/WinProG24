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

        }

        //Commands

        public ICommand MouseDownNodeCommand { get; }
        public ICommand MouseMoveNodeCommand { get; }
        public ICommand MouseUpNodeCommand { get; }

        private void MouseDownNode(MouseButtonEventArgs e)
        {
            var node = TargetShape(e);
            var mousePosition = RelativeMousePosition(e);

            initialMousePosition = mousePosition;
            initialNodePosition = new Point(node.X, node.Y);

            send(node);

            e.MouseDevice.Target.CaptureMouse();
        }

        private void MouseUpNode(MouseButtonEventArgs e)
        {
            var node = TargetShape(e);

            e.MouseDevice.Target.ReleaseMouseCapture();
            if (node.X < 0 || node.Y < 0)
            {
                MessageBox.Show("wtf");
                node.X = initialNodePosition.X;
                node.Y = initialNodePosition.Y;
                moved = false;
            }
            if (moved && initialNodePosition.X == node.initialX && initialNodePosition.Y == node.initialY)
            {
                AddNode(node);
            }
            moved = true;

        }

        private void MouseMoveNode(MouseEventArgs e)
        {
            // Checks that the mouse is captured and that a line is not being drawn.
            if (Mouse.Captured != null) //&& !isAddingLine)
            {
                // The Shape is gotten from the mouse event.
                var node = TargetShape(e);
                // The mouse position relative to the target of the mouse event.
                var mousePosition = RelativeMousePosition(e);

                // The Shape is moved by the offset between the original and current mouse position.
                // The View (GUI) is then notified by the Shape, that its properties have changed.
                var tempX = initialNodePosition.X + (mousePosition.X - initialMousePosition.X);
                var tempY = initialNodePosition.Y + (mousePosition.Y - initialMousePosition.Y);
                if ((initialNodePosition.X == node.initialX && initialNodePosition.Y == node.initialY) || !(tempX < 0 || tempY < 0))
                {
                    node.X = tempX;
                    node.Y = tempY;
                }

            }
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


        //Non important functions
        private Node TargetShape(MouseEventArgs e)
        {
            var nodeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            return (Node)nodeVisualElement.DataContext;
        }

        private Point RelativeMousePosition(MouseEventArgs e)
        {
            var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            var canvas = FindParentOfType<Canvas>(shapeVisualElement);
            return Mouse.GetPosition(canvas);
        }

        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }

    }
}
