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
    public abstract class MainViewModelBase : ViewModelBase
    {
        // Select box

        private Point SelectionBoxStart;

        public double SelectionBoxX { get; set; }
        public double SelectionBoxY { get; set; }
        public double SelectionBoxWidth { get; set; }
        public double SelectionBoxHeight { get; set; }

        // end select box
        public static ObservableCollection<NodeViewModel> Nodes { get; set; } 
        public static ObservableCollection<LineViewModel> Lines { get; set; }
        public static bool isAddingLine { get; set; }
        public static NodeViewModel fromNode { get; set; }

        public static Point initialMousePosition { get; set; }
        public static Point initialNodePosition { get; set; }

        private static Boolean moved = true;

        public MainViewModelBase()
        {
            MouseLeftButtonDown = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);
            MouseLeftButtonUp = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            //MouseDoubleClick = new RelayCommand<MouseButtonEventArgs>(e => Debug.WriteLine(e));
            Mdc = new RelayCommand<MouseButtonEventArgs>(e => Debug.WriteLine(e));

            // Select box
            MouseDownCanvasCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownCanvas);
            MouseMoveCanvasCommand = new RelayCommand<MouseEventArgs>(MouseMoveCanvas);
            MouseUpCanvasCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpCanvas);

        }

        //Commands

        public ICommand MouseLeftButtonDown { get; }
        public ICommand MouseMoveNodeCommand { get; }
        public ICommand MouseLeftButtonUp { get; }
        public ICommand Mdc { get; }

        // Select box
        public ICommand MouseDownCanvasCommand { get; }
        public ICommand MouseMoveCanvasCommand { get; }
        public ICommand MouseUpCanvasCommand { get; }

        private void MouseDownCanvas(MouseButtonEventArgs e)
        {
            //if (!isAddingLine)
            //{
            Console.Write("blaa");
            Console.WriteLine(e.MouseDevice.Target);
            SelectionBoxStart = Mouse.GetPosition(e.MouseDevice.Target);
            SelectionBoxStart.X -= SidePanelViewModel.WIDTH;    
            e.MouseDevice.Target.CaptureMouse();
            //}
        }

        private void MouseMoveCanvas(MouseEventArgs e)
        {
            if (Mouse.Captured != null && !isAddingLine)
            {
                var SelectionBoxNow = Mouse.GetPosition(e.MouseDevice.Target);
                SelectionBoxX = Math.Min(SelectionBoxStart.X, SelectionBoxNow.X);
                SelectionBoxY = Math.Min(SelectionBoxStart.Y, SelectionBoxNow.Y);
                SelectionBoxWidth = Math.Abs(SelectionBoxNow.X - SelectionBoxStart.X);
                SelectionBoxHeight = Math.Abs(SelectionBoxNow.Y - SelectionBoxStart.Y);
                RaisePropertyChanged(() => SelectionBoxX);
                RaisePropertyChanged(() => SelectionBoxY);
                RaisePropertyChanged(() => SelectionBoxWidth);
                RaisePropertyChanged(() => SelectionBoxHeight);
            }
        }

        private void MouseUpCanvas(MouseButtonEventArgs e)
        {
            if (!isAddingLine)
            {
                var SelectionBoxEnd = Mouse.GetPosition(e.MouseDevice.Target);
                var smallX = Math.Min(SelectionBoxStart.X, SelectionBoxEnd.X);
                var smallY = Math.Min(SelectionBoxStart.Y, SelectionBoxEnd.Y);
                var largeX = Math.Max(SelectionBoxStart.X, SelectionBoxEnd.X);
                var largeY = Math.Max(SelectionBoxStart.Y, SelectionBoxEnd.Y);
                //foreach (var s in Shapes)
                //    s.IsMoveSelected = s.CanvasCenterX > smallX && s.CanvasCenterX < largeX && s.CanvasCenterY > smallY && s.CanvasCenterY < largeY;

                SelectionBoxX = SelectionBoxY = SelectionBoxWidth = SelectionBoxHeight = 0;
                RaisePropertyChanged(() => SelectionBoxX);
                RaisePropertyChanged(() => SelectionBoxY);
                RaisePropertyChanged(() => SelectionBoxWidth);
                RaisePropertyChanged(() => SelectionBoxHeight);
                e.MouseDevice.Target.ReleaseMouseCapture();
            }
        }


        public void AddLine( NodeViewModel to)
        {
            fromNode.Color = fromNode.PreColor;
            isAddingLine = false;
            Lines.Add(new LineViewModel(new Line()) { From = fromNode, To = to });
            fromNode = null;
        }

        public void AddNode(NodeViewModel node)
        {
            Nodes.Add(node);
        }
        public void MouseDoubleClickNode(MouseButtonEventArgs e)
        {
            var node = TargetShape(e);
            if(!(node.isTextBoxVisible == Visibility.Visible))
            {
                
                node.isTextBoxVisible = Visibility.Visible;
            } else
            {
                node.isTextBoxVisible = Visibility.Hidden;
            }
            MessageBox.Show("lol");
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

            e.MouseDevice.Target.ReleaseMouseCapture();


            //if (node.X < 0 || node.Y < 0)
            //{

            //                node.X = initialNodePosition.X;
            //              node.Y = initialNodePosition.Y;
            //            moved = false;
            //         }
            //if (moved && initialNodePosition.X == node.initialX && initialNodePosition.Y == node.initialY)
            //{
            //      AddNode(node);
            // }
            //moved = true;

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

        }

        private void MouseMoveNode(MouseEventArgs e)
        {
            if (Mouse.Captured != null)
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
