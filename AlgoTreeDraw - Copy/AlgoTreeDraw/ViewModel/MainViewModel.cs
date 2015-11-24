using AlgoTreeDraw.Model;
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
            if(!nodeClicked && !isAddingLine)
            {
                Console.WriteLine("MDCanvas called");
                clearSelectedNodes();
                SelectionBoxStart = Mouse.GetPosition(e.MouseDevice.Target);
                e.MouseDevice.Target.CaptureMouse();
                MouseDownCanvasCalled = true;
            }
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
                e.MouseDevice.Target.ReleaseMouseCapture();
            }
        }


    }
}