using AlgoTreeDraw.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AlgoTreeDraw.ViewModel
{
    public class SidePanelViewModel : ViewModelBase
    {

        public ObservableCollection<Node> Nodes { get; set; }

        private bool isAddingLine = false;

        public int HEIGHT {get{ return 350;}}
        public int WIDTH { get { return 240; } }

        Brush _background;

        public Brush BackgroundAddLine
        {
            get { return isAddingLine ? Brushes.Pink : Brushes.LightGreen; }
            set { _background = value; }
        }



        public SidePanelViewModel()
        {
            Nodes = new ObservableCollection<Node>() {
                new BST() { X = 0, Y = 0, diameter = 50}
            };

            MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            AddLineCommand = new RelayCommand(AddLineClicked);

            mecs = new MouseEventCommands();
      
            Messenger.Default.Register<ALBstToSide>(this, (action) => ReceiveALBstToSide(action));
        }

        //Commands
        public ICommand MouseDownNodeCommand { get; }
        public ICommand MouseMoveNodeCommand { get; }
        public ICommand MouseUpNodeCommand { get; }
        public ICommand AddLineCommand { get; }

        private MouseEventCommands mecs { get; set; }

        private void MouseDownNode(MouseButtonEventArgs e)
        {
            mecs.MouseDownNode(e);
        }

        private void MouseUpNode(MouseButtonEventArgs e)
        {
            var node = mecs.MouseUpNode(e);
            
            if (isInsideCanvas(node))
            {
                Messenger.Default.Send<NodeMessage>(new NodeMessage() { node = new BST() { X = node.X - WIDTH, Y = node.Y, diameter = node.diameter} });
            }
            node.X = MouseEventCommands.initialNodePosition.X;
            node.Y = MouseEventCommands.initialNodePosition.Y;
        }

        private void MouseMoveNode(MouseEventArgs e)
        {
            mecs.MouseMoveNode(e);
        }

        public bool isInsideCanvas(Node node)
        {
            return node.X > WIDTH;
        }

        private void AddLineClicked()
        {
            isAddingLine = !isAddingLine;
            Messenger.Default.Send<ALSideToBst>(new ALSideToBst() { isAddingLine = isAddingLine });
            RaisePropertyChanged("BackgroundAddLine");
        }

        private object ReceiveALBstToSide(ALBstToSide action)
        {
            isAddingLine = action.isAddingLine;
            
            RaisePropertyChanged("BackgroundAddLine");
            return null;
        }
    }
}
