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
using Xceed.Wpf.Toolkit;

namespace AlgoTreeDraw.ViewModel
{
    public abstract class MainViewModelBase : ViewModelBase
    {
        public UndoRedo undoRedo
        {
            get; set;
        } = UndoRedo.Instance;
        public static ObservableCollection<NodeViewModel> Nodes { get; set; } 
        public static ObservableCollection<LineViewModel> Lines { get; set; }
        public static NodeViewModel fromNode { get; set; }
        public static Color ChosenColor { get; set; }
        //Commands


        public ICommand MouseLeftButtonDown { get; }
        public ICommand MouseMoveNodeCommand { get; }
        public ICommand MouseLeftButtonUp { get; }
        public ICommand Mdc { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        public ICommand DoneEditing { get; }
        public static Point initialMousePosition { get; set; }
        public static Point initialNodePosition { get; set; }

        private static Boolean moved = true;
        private NodeViewModel editNode { get; set; }
        private bool hasEdited { get; set; }

        //Tools
        public static bool isAddingLine { get; set; }

        public static bool isChangingColor { get; set; }
        public static bool isMarking { get; set; }
        public static bool hasmarkedSomething { get; set; }

        



        public MainViewModelBase()
        {
            MouseLeftButtonDown = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);
            MouseLeftButtonUp = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            //MouseDoubleClick = new RelayCommand<MouseButtonEventArgs>(e => Debug.WriteLine(e));
            Mdc = new RelayCommand<MouseButtonEventArgs>(e => Debug.WriteLine(e));
            UndoCommand = new RelayCommand<int>(undoRedo.Undo, undoRedo.CanUndo);
            RedoCommand = new RelayCommand<int>(undoRedo.Redo, undoRedo.CanRedo);
            DoneEditing = new RelayCommand(_DoneEditing);

        }


        public void _DoneEditing()
        {
            editNode.IsEditing = Visibility.Hidden;
            editNode.IsNotEditing = Visibility.Visible;
        }


        public void AddLine( NodeViewModel to)
        {
            fromNode.Color = fromNode.PreColor;
            isAddingLine = false;
            LineViewModel tempLine = new LineViewModel(new Line()) { From = fromNode, To = to };
            undoRedo.InsertInUndoRedo(new AddLineCommand(Lines,tempLine));
            fromNode = null;
        }

        public void AddNode(NodeViewModel node)
        {
            Nodes.Add(node);
        }
                
        public NodeViewModel MouseUpNodeSP2(MouseButtonEventArgs e)
        {
            var node = TargetShape(e);

            e.MouseDevice.Target.ReleaseMouseCapture();

            return node;
        }

        public void MouseUpNode(MouseButtonEventArgs e)
        {

            var node = TargetShape(e);


            var mousePosition = RelativeMousePosition(e);

            node.X = initialNodePosition.X;
            node.Y = initialNodePosition.Y;

            //Later we want to move many selected nodes
            var temp = new List<NodeViewModel>() { node};

            undoRedo.InsertInUndoRedo(new MoveNodeCommand(temp, mousePosition.X - initialMousePosition.X, mousePosition.Y - initialMousePosition.Y));

            e.MouseDevice.Target.ReleaseMouseCapture();

            if (isChangingColor)
            {
                node.Color = new SolidColorBrush(ChosenColor);
            }

            if (isAddingLine)
            {
                if (fromNode == null) { fromNode = node; fromNode.Color = Brushes.Blue; }
                else if (!Object.ReferenceEquals(fromNode, node)) { AddLine(node); }
                
            }
        }


        private void MouseDownNode(MouseButtonEventArgs e)
        {

            var node = TargetShape(e);
            var mousePosition = RelativeMousePosition(e);

            initialMousePosition = mousePosition;
            initialNodePosition = new Point(node.X, node.Y);

            e.MouseDevice.Target.CaptureMouse();
            if (e.ClickCount == 2 && e.LeftButton == MouseButtonState.Pressed)
            {
                node.IsEditing = Visibility.Visible;
                node.IsNotEditing = Visibility.Hidden;
                editNode = node;
            }

        }

        private void MouseMoveNode(MouseEventArgs e)
        {
            if (Mouse.Captured != null && !isAddingLine)
            {

                var node = TargetShape(e);

                var mousePosition = RelativeMousePosition(e);

                var tempX = initialNodePosition.X + (mousePosition.X - initialMousePosition.X);
                var tempY = initialNodePosition.Y + (mousePosition.Y - initialMousePosition.Y);
                if ( !(tempX < 0 || tempY < 0))
                {
                    node.X = tempX;
                    node.Y = tempY;
                }
                
            }
        }

        //Non important functions
        public NodeViewModel TargetShape(MouseEventArgs e)
        {
            var nodeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            return (NodeViewModel)nodeVisualElement.DataContext;
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
