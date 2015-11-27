using AlgoTreeDraw.Model;
using AlgoTreeDraw.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AlgoTreeDraw.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : MainViewModelBase
    {
        //public SidePanelViewModel SidePanelViewModel { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 
        private PullCanvas pullCanvas { get; set; }

        private Point SelectionBoxStart;

        public double SelectionBoxX { get; set; }
        public double SelectionBoxY { get; set; }
        public double SelectionBoxWidth { get; set; }
        public double SelectionBoxHeight { get; set; }

        public bool MouseDownCanvasCalled = false;

        public ICommand MouseDownCanvasCommand { get; }
        public ICommand MouseMoveCanvasCommand { get; }
        public ICommand MouseUpCanvasCommand { get; }

        public MainViewModel() 
        {
            pullCanvas = new PullCanvas(false, false, false);

            MouseDownCanvasCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownCanvas);
            MouseMoveCanvasCommand = new RelayCommand<MouseEventArgs>(MouseMoveCanvas);
            MouseUpCanvasCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpCanvas);
        }

        private void MouseDownCanvas(MouseButtonEventArgs e)
        {

            Point CurrentMousePosition = Mouse.GetPosition(e.MouseDevice.Target);
            if (CurrentMousePosition.X <= CanvasWidth + 10 && CurrentMousePosition.X >= CanvasWidth - 10 && CurrentMousePosition.Y <= CanvasHeight + 10 && CurrentMousePosition.Y >= CanvasHeight - 10)
            {
                pullCanvas = new PullCanvas(true,true,true);
            }
            else if(CurrentMousePosition.X <= CanvasWidth + 10 && CurrentMousePosition.X >= CanvasWidth - 10)
            {
                pullCanvas = new PullCanvas(true, true, false);
            }
            else if(CurrentMousePosition.Y <= CanvasHeight + 10 && CurrentMousePosition.Y >= CanvasHeight - 10)
            {
                pullCanvas = new PullCanvas(true, false, true);
            }
            else
            {
                if (!nodeClicked)
                {
                    clearSelectedNodes();
                    SelectionBoxStart = Mouse.GetPosition(e.MouseDevice.Target);
                    MouseDownCanvasCalled = true;
                    _DoneEditing();
                    isAddingLine = false;
                    isChangingColor = false;
                    isChangingColorText = false;
                    Messenger.Default.Send(Cursors.Arrow);
                    if (fromNode != null) { fromNode.Color = fromNode.PreColor; fromNode = null; }

                }
                else
                {
                    var node = TargetShape(e);
                    if(node != editNode)
                    {
                        _DoneEditing();
                    }
                }
            }
            e.MouseDevice.Target.CaptureMouse();
        }

        private void MouseMoveCanvas(MouseEventArgs e)
        {
            if (Mouse.Captured != null && MouseDownCanvasCalled)
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

            if(Mouse.Captured != null)
            {
                if(pullCanvas.isPullingCanvas)
                {
                    Point CurrentMousePosition = Mouse.GetPosition(e.MouseDevice.Target);
                    if(pullCanvas.isPullingWidth) CanvasWidth = (int)CurrentMousePosition.X;
                    if (pullCanvas.isPullingHeight) CanvasHeight = (int)CurrentMousePosition.Y;

                }
            }
        }

        private void MouseUpCanvas(MouseButtonEventArgs e)
        {
            if(Mouse.Captured != null)
            {
                if (!isAddingLine && MouseDownCanvasCalled)
                {
                    Console.WriteLine("Inside");
                    MouseDownCanvasCalled = false;
                    var SelectionBoxEnd = Mouse.GetPosition(e.MouseDevice.Target);
                    var smallX = Math.Min(SelectionBoxStart.X, SelectionBoxEnd.X);
                    var smallY = Math.Min(SelectionBoxStart.Y, SelectionBoxEnd.Y);
                    var largeX = Math.Max(SelectionBoxStart.X, SelectionBoxEnd.X);
                    var largeY = Math.Max(SelectionBoxStart.Y, SelectionBoxEnd.Y);
                    foreach (NodeViewModel n in Nodes)
                        if (!(n.X > largeX || n.X + n.Diameter < smallX || n.Y > largeY || n.Y + n.Diameter < smallY))
                        {
                            addToSelectedNodes(n);
                        }

                    SelectionBoxX = SelectionBoxY = SelectionBoxWidth = SelectionBoxHeight = 0;
                    RaisePropertyChanged(() => SelectionBoxX);
                    RaisePropertyChanged(() => SelectionBoxY);
                    RaisePropertyChanged(() => SelectionBoxWidth);
                    RaisePropertyChanged(() => SelectionBoxHeight);

                }

                if (pullCanvas.isPullingCanvas)
                {
                    Func<NodeViewModel, bool> isInsideCanvasWidth = n => n.X + n.Diameter < CanvasWidth && n.X > 0;
                    Func<NodeViewModel, bool> isInsideCanvasHeight = n => n.Y + n.Diameter < CanvasHeight && n.Y > 0;
                    foreach (var n in Nodes)
                    {
                        if (!isInsideCanvasWidth(n)) CanvasWidth = (int)n.X + (int)n.Diameter;
                        if (!isInsideCanvasHeight(n)) CanvasHeight = (int)n.Y + (int)n.Diameter;
                    }
                    if (CanvasHeight < 0) CanvasHeight = 100;
                    if (CanvasWidth < 0) CanvasWidth = 100;
                    pullCanvas = new PullCanvas(false, false, false);
                }
                e.MouseDevice.Target.ReleaseMouseCapture();
            }
            
            
        }

        class PullCanvas {
            public bool isPullingCanvas { get; }
            public bool isPullingWidth { get; }
            public bool isPullingHeight { get; }
            public PullCanvas(bool isPullingCanvas, bool isPullingWidth, bool isPullingHeight)
            {
                this.isPullingCanvas = isPullingCanvas;
                this.isPullingWidth = isPullingWidth;
                this.isPullingHeight = isPullingHeight;
            }
        }
        
    }
}