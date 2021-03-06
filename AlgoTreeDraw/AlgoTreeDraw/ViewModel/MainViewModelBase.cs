﻿using AlgoTreeDraw.Model;
using System;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.CommandWpf;
using AlgoTreeDraw.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics;

namespace AlgoTreeDraw.ViewModel
{
    public abstract class MainViewModelBase : ViewModelBase
    {
        class ZoomValue
        {
            double value { get; set; } = 1;
            public ZoomValue(double value)
            {
                this.value = value;
            }
        }
        public UndoRedo undoRedo {get; set;} = UndoRedo.Instance;
        public static ObservableCollection<NodeViewModel> Nodes { get; set; } = new ObservableCollection<NodeViewModel>();
        public static ObservableCollection<LineViewModel> Lines { get; set; } = new ObservableCollection<LineViewModel>();
        public static NodeViewModel fromNode { get; set; }
        public static Color ChosenColor { get; set; }
        private static double _zoomValue = 1;
        public double zoomValue { get { return _zoomValue; } set {_zoomValue = value; Messenger.Default.Send(new ZoomValue(_zoomValue)); RaisePropertyChanged(); } }
        private bool newLineError = false;

        public static List<NodeViewModel> selectedNodes = new List<NodeViewModel>();
        public static List<NodeViewModel> copiedNodes = new List<NodeViewModel>();
        public static List<LineViewModel> copiedLines = new List<LineViewModel>();
        public static List<NodeViewModel> mostRecentPastedNodes = new List<NodeViewModel>();
        //Commands


        public ICommand MouseLeftButtonDown { get; }
        public ICommand MouseMoveNodeCommand { get; }
        public ICommand MouseLeftButtonUp { get; }
        public ICommand Mdc { get; }
        public ICommand DoneEditing { get; }
        public ICommand DeleteKeyPressed { get; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }
        public ICommand CutCommand { get; }
        public ICommand ZoomInCommand { get; }
        public ICommand ZoomOutCommand { get; }


        public static Point initialMousePosition { get; set; }

        private int _HEIGHT = 1000;
        private int _WIDTH = 1000;
        private static int canvasWidth = 900;
        private static int canvasHeight = 600;
        public int CanvasWidth { get { return canvasWidth; } set { canvasWidth = value; RaisePropertyChanged();} }
        public int CanvasHeight { get { return canvasHeight; } set { canvasHeight = value; RaisePropertyChanged(); } }
    
        public int HEIGHT { get { return _HEIGHT; } set { _HEIGHT = value; RaisePropertyChanged();} }
        public int WIDTH { get { return _WIDTH; } set { _WIDTH = value; RaisePropertyChanged();}}

        //select
        public bool nodeClicked = false;

        private static Boolean moved = true;

        private static NodeViewModel _editNode;
        public NodeViewModel editNode { get { return _editNode; } set { _editNode = value; } }
        private bool hasEdited { get; set; }

        //Tools
        public static bool isAddingLine { get; set; }

        public static bool isChangingColor { get; set; }
        public static bool isChangingColorText { get; set; }
        public static bool isMarking { get; set; }
        public static bool hasmarkedSomething { get; set; }

        public static double _VOff = 0;
        public double VOff
        {
            get { return _VOff; }
            set { _VOff = value; }
        }

        public MainViewModelBase()
        {
            MouseLeftButtonDown = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);
            MouseLeftButtonUp = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            Mdc = new RelayCommand<MouseButtonEventArgs>(e => Debug.WriteLine(e));
            DoneEditing = new RelayCommand(_DoneEditing);
            CopyCommand = new RelayCommand(copyClicked);
            PasteCommand = new RelayCommand(pasteClicked);
            CutCommand = new RelayCommand(cutClicked);
            ZoomInCommand = new RelayCommand(ZoomIn);
            ZoomOutCommand = new RelayCommand(ZoomOut);

            DeleteKeyPressed = new RelayCommand(RemoveNodeKeybordDelete);
            Messenger.Default.Register<ZoomValue>(this, UpdateZoom);
        }

        private void ZoomIn()
        {
            zoomValue += 0.2;
        }

        private void ZoomOut()
        {
            zoomValue -= 0.2;
        }

        private void UpdateZoom(ZoomValue value)
        {
            RaisePropertyChanged("zoomValue");
        }

        public void cutClicked()
        {
            copyClicked();
            RemoveNodeKeybordDelete();
        }

        public void copyClicked()
        {
            copiedLines.Clear();
            copiedNodes.Clear();
            foreach (LineViewModel l in Lines)
                if (selectedNodes.Contains(l.To) && selectedNodes.Contains(l.From) && !copiedLines.Contains(l))
                {
                    LineViewModel line = new LineViewModel(new Line() { From = l.From.Node, To = l.To.Node }) { From = l.From, To = l.To };
                    copiedLines.Add(line);
                }

            foreach (NodeViewModel n in selectedNodes)
            {
                NodeViewModel node = n.newNodeViewModel();
                copiedNodes.Add(node);
                foreach (LineViewModel l in copiedLines)
                {
                    if (node.X == l.From.X && node.Y == l.From.Y)
                    {
                        l.From = node;
                    }
                    else if (node.X == l.To.X && node.Y == l.To.Y)
                        l.To = node;
                }
            }


        }

        public void pasteClicked()
        {
            undoRedo.InsertInUndoRedo(new PasteCommand(Nodes, copiedNodes, selectedNodes,copiedLines,Lines));
        }

        public void RemoveNodeKeybordDelete()
        {
            isAddingLine = false;
            undoRedo.InsertInUndoRedo(new DeleteNodeCommand(Nodes, selectedNodes, Lines));
            selectedNodes.Clear();
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

        public void _DoneEditingT234(int type)
        {
            if (editNode != null)
            {
                T234ViewModel node= ((T234ViewModel)editNode);
                if (type == 0)
                {
                    if (node.TxtOne != node.PreTxtOne)
                    {
                        undoRedo.InsertInUndoRedo(new TextChangeCommandT234(node, node.TxtOne, node.PreTxtOne, type));
                    }
                } else if(type == 1)
                {
                    if (node.TxtTwo != node.PreTxtTwo)
                    {
                        undoRedo.InsertInUndoRedo(new TextChangeCommandT234(node, node.TxtTwo, node.PreTxtTwo, type));
                    }
                } else
                {
                    if (node.TxtThree != node.PreTxtThree)
                    {
                        undoRedo.InsertInUndoRedo(new TextChangeCommandT234(node, node.TxtThree, node.PreTxtThree, type));
                    }
                }
                NodeBoxUpdate();
                editNode = null;
            }
        }

        public void NodeBoxUpdate()
        {
            if(editNode != null)
            {
                editNode.IsEditing = Visibility.Hidden;
                editNode.IsNotEditing = Visibility.Visible;
                
            }
        }
        
        public void _DoneEditing()
        {
            if(editNode != null)
            {
                
                if (editNode.TxtOne != editNode.PreTxtOne)
                {
                    undoRedo.InsertInUndoRedo(new TextChangeCommand(editNode, editNode.TxtOne, editNode.PreTxtOne));
                }
                NodeBoxUpdate();
                editNode = null;
            }
            
        }

        public void AddLine( NodeViewModel to)
        {
            
            fromNode.Color = fromNode.PreColor;
            if (fromNode.neighbours.Contains(to))
                newLineError = true;
            else
            {
                LineViewModel tempLine = new LineViewModel(new Line()) { From = fromNode, To = to };
                undoRedo.InsertInUndoRedo(new AddLineCommand(Lines,tempLine,fromNode,to));

            }
            fromNode = null;
        }

        public void AddNode(NodeViewModel node)
        {
            if (!(node.X + node.Diameter > CanvasWidth || node.Y + node.Diameter > CanvasHeight)) undoRedo.InsertInUndoRedo(new AddNodeCommand(Nodes, node));
        }
                
        public NodeViewModel MouseUpNodeSP2(MouseButtonEventArgs e)
        {
            var node = TargetShape(e);

            e.MouseDevice.Target.ReleaseMouseCapture();

            return node;
        }

        public void MouseUpNode(MouseButtonEventArgs e)
        {

            nodeClicked = false;
            var node = TargetShape(e);
  


            if (isChangingColor)
            {
                undoRedo.InsertInUndoRedo(new ChangeColorCommand(node, new SolidColorBrush(ChosenColor), node.Color));
            }

            else if (isChangingColorText)
            {
                undoRedo.InsertInUndoRedo(new ChangeColorTextCommand(node, new SolidColorBrush(ChosenColor), node.PreColorOfText));
            }
            else if (isAddingLine)
            {
                if (fromNode == null) { fromNode = node; fromNode.Color = Brushes.Blue; }
                else if (!Object.ReferenceEquals(fromNode, node)) { AddLine(node); }
                
            }
            else if (node==editNode)
            {

            }
            else
            {
                bool isOutside = false;
                foreach (NodeViewModel n1 in selectedNodes)
                {
                    if (n1.X < 0 || n1.Y < 0)
                    {
                        isOutside = true;
                        break;
                    }
                }
                if (isOutside)
                {
                    foreach (NodeViewModel n in selectedNodes)
                    {
                        n.X = n.initialNodePosition.X;
                        n.Y = n.initialNodePosition.Y;
                    }
                } else { 
                        var mousePosition = RelativeMousePosition(e);

                        foreach (NodeViewModel n in selectedNodes)
                        {
                            n.X = n.initialNodePosition.X;
                            n.Y = n.initialNodePosition.Y;
                        }

                        if (!(initialMousePosition.X == mousePosition.X && initialMousePosition.Y == mousePosition.Y)) //Only when it actually moves
                    {
                        undoRedo.InsertInUndoRedo(new MoveNodeCommand(selectedNodes, mousePosition.X - initialMousePosition.X, mousePosition.Y - initialMousePosition.Y));
                        foreach (var n in selectedNodes)
                        {
                            if (n.X + n.Diameter > CanvasWidth)
                                CanvasWidth = (int)(n.X + n.Diameter);
                            if (n.Y + n.Diameter > CanvasHeight)
                                CanvasHeight = (int)(n.Y + n.Diameter);
                        }
                        }
                }
            }
                e.MouseDevice.Target.ReleaseMouseCapture();
            if(newLineError)
            {
                System.Windows.MessageBox.Show("There is a line here already :):):)");
                newLineError = false;
            }

        }


        private void MouseDownNode(MouseButtonEventArgs e)
        {

            nodeClicked = true;
            var node = TargetShape(e);
            var mousePosition = RelativeMousePosition(e);

            initialMousePosition = mousePosition;

            if(!selectedNodes.Contains(node))
            {
                clearSelectedNodes();
                addToSelectedNodes(node);
            }

            foreach (NodeViewModel n in selectedNodes)
            {
                n.initialNodePosition.X = n.X;
                n.initialNodePosition.Y = n.Y;
            }
            
            e.MouseDevice.Target.CaptureMouse();
            if (e.ClickCount == 2 && e.LeftButton == MouseButtonState.Pressed && !(node.ID == 0 || node.ID == 1 || node.ID == 2))
            {
                node.IsEditing = Visibility.Visible;
                node.IsNotEditing = Visibility.Hidden;
                editNode = node;
            }

            var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            var canvas = FindParentOfType<Canvas>(shapeVisualElement);
        }

        private void MouseMoveNode(MouseEventArgs e)
        {
            var node = TargetShape(e);
            if (Mouse.Captured != null && !isAddingLine && !isChangingColor && !isChangingColorText && (node!=editNode || node.ID == 0 || node.ID == 1 || node.ID == 2))
            {

                var mousePosition = RelativeMousePosition(e);

                foreach(NodeViewModel n in selectedNodes)
                {
                    n.X = n.initialNodePosition.X + (mousePosition.X - initialMousePosition.X);
                    n.Y = n.initialNodePosition.Y + (mousePosition.Y - initialMousePosition.Y);
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
