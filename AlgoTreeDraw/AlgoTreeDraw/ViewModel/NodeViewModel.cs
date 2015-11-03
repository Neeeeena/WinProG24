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
    public class NodeViewModel : MainViewModelBase
    {

        public static Point initialMousePosition { get; set; }
        public static Point initialNodePosition { get; set; }
        public Node LineFrom = null;
        private static Boolean moved = true;
        public NodeViewModel()
        {


            MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);


        }

        //Commands

        public ICommand MouseDownNodeCommand { get; }
        public ICommand MouseMoveNodeCommand { get; }
        public ICommand MouseUpNodeCommand { get; }



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

            if (isAddingLine)
            {
                if (LineFrom == null) { LineFrom = node; LineFrom.Color = Brushes.Blue; }
                else if (!Object.ReferenceEquals(LineFrom, node)) { AddLine(node); }

            }

        }

        private void MouseDownNode(MouseButtonEventArgs e)
        {
            var node = TargetShape(e);
            var mousePosition = RelativeMousePosition(e);

            initialMousePosition = mousePosition;
            initialNodePosition = new Point(node.X, node.Y);

            e.MouseDevice.Target.CaptureMouse();

        }


        private void MouseMoveNode(MouseEventArgs e)
        {
            if (Mouse.Captured != null)
            {

                var node = TargetShape(e);

                var mousePosition = RelativeMousePosition(e);

                var tempX = initialNodePosition.X + (mousePosition.X - initialMousePosition.X);
                var tempY = initialNodePosition.Y + (mousePosition.Y - initialMousePosition.Y);
                if ((initialNodePosition.X == node.initialX && initialNodePosition.Y == node.initialY) || !(tempX < 0 || tempY < 0))
                {
                    node.X = tempX;
                    node.Y = tempY;
                }
            }
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
