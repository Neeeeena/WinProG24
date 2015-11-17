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

        public static List<NodeViewModel> selectedNodes = new List<NodeViewModel>();
        public static List<NodeViewModel> copiedNodes = new List<NodeViewModel>();
        public static List<LineViewModel> copiedLines = new List<LineViewModel>();
        public static List<NodeViewModel> mostRecentPastedNodes = new List<NodeViewModel>();
        //Commands


        public ICommand MouseLeftButtonDown { get; }
        public ICommand MouseMoveNodeCommand { get; }
        public ICommand MouseLeftButtonUp { get; }
        public ICommand Mdc { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        public ICommand DoneEditing { get; }
        public ICommand DeleteKeyPressed { get; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }

        public static Point initialMousePosition { get; set; }

        private int _HEIGHT = 1000;
        private int _WIDTH = 1000;

        private Brush _TEST = Brushes.Black;
        public Brush TEST { get { return _TEST; } set { _TEST = value; RaisePropertyChanged(); } }
    
        public int HEIGHT { get { return _HEIGHT; } set { _HEIGHT = value; RaisePropertyChanged(); RaisePropertyChanged(() => TEST); } }
        public int WIDTH { get { return _WIDTH; } set { _WIDTH = value; RaisePropertyChanged(); RaisePropertyChanged(() => TEST); } }

        //public static Point initialNodePosition { get; set; }

        //select
        public bool nodeClicked = false;

        private static Boolean moved = true;
        private NodeViewModel editNode { get; set; }
        private bool hasEdited { get; set; }

        //Tools
        public static bool isAddingLine { get; set; }

        public static bool isChangingColor { get; set; }
        public static bool isChangingColorText { get; set; }
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
            CopyCommand = new RelayCommand(copyClicked);
            PasteCommand = new RelayCommand(pasteClicked);

            DeleteKeyPressed = new RelayCommand<KeyEventArgs>(RemoveNodeKeybordDelete);

        }

        public void copyClicked()
        {
            copiedNodes.Clear();
            foreach (NodeViewModel n in selectedNodes)
            {
                NodeViewModel node = n.newNodeViewModel();
                copiedNodes.Add(node);
            }
           
        }

        public void pasteClicked()
        {
            undoRedo.InsertInUndoRedo(new PasteCommand(Nodes, copiedNodes, selectedNodes, mostRecentPastedNodes));
        }

        public void RemoveNodeKeybordDelete(KeyEventArgs e)
        {
            undoRedo.InsertInUndoRedo(new DeleteNodeCommand(Nodes, selectedNodes, Lines));
        }

        //Select
        public void addToSelectedNodes(NodeViewModel n)
        {
            selectedNodes.Add(n);
            
            n.BorderColor = Brushes.DarkBlue;
            n.BorderThickness = 4;
        }

        public void clearSelectedNodes()
        {
            foreach(NodeViewModel n in selectedNodes)
            {
                n.BorderColor = Brushes.Black;
                n.BorderThickness = 1;
            }
            selectedNodes.Clear();
        }

        public void makePretty()
        {
            Nodes.ElementAt(0).makePretty();
        }

        public void _DoneEditing()
        {
            if(editNode != null)
            {
                editNode.IsEditing = Visibility.Hidden;
                editNode.IsNotEditing = Visibility.Visible;
            }
            
        }


        public void AddLine( NodeViewModel to)
        {
            fromNode.Color = fromNode.PreColor;
            isAddingLine = false;
            LineViewModel tempLine = new LineViewModel(new Line()) { From = fromNode, To = to };
            undoRedo.InsertInUndoRedo(new AddLineCommand(Lines,tempLine));
            fromNode.addNeighbour(to.Node);
            fromNode = null;
        }

        public void AddNode(NodeViewModel node)
        {
            undoRedo.InsertInUndoRedo(new AddNodeCommand(Nodes, node));
        }
                
        public NodeViewModel MouseUpNodeSP2(MouseButtonEventArgs e)
        {
            var node = TargetShape(e);

            e.MouseDevice.Target.ReleaseMouseCapture();

            return node;
        }

        public void MouseUpNode(MouseButtonEventArgs e)
        {
            Console.WriteLine("MUNode called");

            nodeClicked = false;
            var node = TargetShape(e);
  
            foreach(NodeViewModel n in selectedNodes)
            {
                n.X = n.initialNodePosition.X;
                n.Y = n.initialNodePosition.Y;
            }

            if (isChangingColor)
            {
                undoRedo.InsertInUndoRedo(new ChangeColorCommand(node, new SolidColorBrush(ChosenColor),node.Color));
                isChangingColor = false;
            }

            if(isChangingColorText)
            {
                undoRedo.InsertInUndoRedo(new ChangeColorTextCommand(node, new SolidColorBrush(ChosenColor), node.PreColorOfText));
            }

            if (isAddingLine)
            {
                if (fromNode == null) { fromNode = node; fromNode.Color = Brushes.Blue; }
                else if (!Object.ReferenceEquals(fromNode, node)) { AddLine(node); }
                
            }



                var mousePosition = RelativeMousePosition(e);


                undoRedo.InsertInUndoRedo(new MoveNodeCommand(selectedNodes, mousePosition.X - initialMousePosition.X, mousePosition.Y - initialMousePosition.Y));

                e.MouseDevice.Target.ReleaseMouseCapture();
            
        }


        private void MouseDownNode(MouseButtonEventArgs e)
        {
            nodeClicked = true;
            Console.WriteLine("MDNode called");
            var node = TargetShape(e);
            var mousePosition = RelativeMousePosition(e);

            initialMousePosition = mousePosition;
            //initialNodePosition = new Point(node.X, node.Y);

            if(!selectedNodes.Contains(node))
            {
                clearSelectedNodes();
                addToSelectedNodes(node);
            }

            foreach (NodeViewModel n in selectedNodes)
            {
                n.initialNodePosition.X = n.X;
                n.initialNodePosition.Y = n.Y;
                Console.WriteLine("initialX = " + n.initialNodePosition.X);
                Console.WriteLine("initialX = " + n.initialNodePosition.X);
            }

            e.MouseDevice.Target.CaptureMouse();
            if (e.ClickCount == 2 && e.LeftButton == MouseButtonState.Pressed)
            {
                node.IsEditing = Visibility.Visible;
                node.IsNotEditing = Visibility.Hidden;
                editNode = node;
                Debug.Write(editNode.Diameter.ToString());
            }

        }

        private void MouseMoveNode(MouseEventArgs e)
        {
            if (Mouse.Captured != null && !isAddingLine && !isChangingColor)
            {

                var mousePosition = RelativeMousePosition(e);

                double tempX;
                double tempY;

                foreach(NodeViewModel n in selectedNodes)
                {
                    tempX = n.initialNodePosition.X + (mousePosition.X - initialMousePosition.X);
                    tempY = n.initialNodePosition.Y + (mousePosition.Y - initialMousePosition.Y);
                    if (!(tempX < 0 || tempY < 0))
                    {
                        n.X = tempX;
                        n.Y = tempY;
                    }
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
