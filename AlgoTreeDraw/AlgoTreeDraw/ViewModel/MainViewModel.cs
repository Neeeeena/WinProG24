using AlgoTreeDraw.Model;
using AlgoTreeDraw.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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

        private int canvasWidth = 500;
        private int canvasHeight = 500;
        public int CanvasWidth { get { return canvasWidth; } set { canvasWidth = value; RaisePropertyChanged(); } }
        public int CanvasHeight { get { return canvasHeight; } set { canvasHeight = value; RaisePropertyChanged(); } }
        public bool isPullingCanvas { get; set; }

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
            // Wat?
            Nodes = new ObservableCollection<NodeViewModel>()
            {

            };

            Lines = new ObservableCollection<LineViewModel>();

            MouseDownCanvasCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownCanvas);
            MouseMoveCanvasCommand = new RelayCommand<MouseEventArgs>(MouseMoveCanvas);
            MouseUpCanvasCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpCanvas);
        }

        private void MouseDownCanvas(MouseButtonEventArgs e)
        {
            Point CurrentMousePosition = Mouse.GetPosition(e.MouseDevice.Target);
            if (CurrentMousePosition.X <= CanvasWidth + 10 && CurrentMousePosition.X >= CanvasWidth - 10 || CurrentMousePosition.Y <= CanvasHeight + 10 && CurrentMousePosition.Y >= CanvasHeight - 10)
            {
                isPullingCanvas = true;
            } else
            {
                if (!nodeClicked && !isAddingLine)
                {
                    clearSelectedNodes();
                    SelectionBoxStart = Mouse.GetPosition(e.MouseDevice.Target);
                    MouseDownCanvasCalled = true;
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
                if(isPullingCanvas)
                {
                    Point CurrentMousePosition = Mouse.GetPosition(e.MouseDevice.Target);
                    CanvasWidth = (int)CurrentMousePosition.X;
                    CanvasHeight = (int)CurrentMousePosition.Y;
                }
            }
        }

        private void MouseUpCanvas(MouseButtonEventArgs e)
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
                    if(!(n.X > largeX || n.X+n.Diameter < smallX || n.Y > largeY || n.Y+n.Diameter < smallY))
                    {
                        addToSelectedNodes(n);
                    }
                Tree yolo = new Tree(selectedNodes);

                SelectionBoxX = SelectionBoxY = SelectionBoxWidth = SelectionBoxHeight = 0;
                RaisePropertyChanged(() => SelectionBoxX);
                RaisePropertyChanged(() => SelectionBoxY);
                RaisePropertyChanged(() => SelectionBoxWidth);
                RaisePropertyChanged(() => SelectionBoxHeight);
                
            }

            if(isPullingCanvas)
            {
                Func<NodeViewModel,bool> isInsideCanvasWidth = n => n.X < canvasWidth && n.X > 0;
                Func<NodeViewModel, bool> isInsideCanvasHeight = n => n.Y < canvasHeight && n.Y > 0;
                foreach (var n in Nodes)
                {
                    if (!isInsideCanvasWidth(n)) CanvasWidth = (int)n.X + (int)n.Diameter;
                    if (!isInsideCanvasHeight(n)) CanvasHeight = (int)n.Y + (int)n.Diameter;
                }
                if (CanvasHeight < 0) CanvasHeight = 100;
                if (CanvasWidth < 0) CanvasWidth = 100; 
                isPullingCanvas = false;
            }
            e.MouseDevice.Target.ReleaseMouseCapture();
        }

        
    }
}