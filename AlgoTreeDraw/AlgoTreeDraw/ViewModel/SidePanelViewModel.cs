using AlgoTreeDraw.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlgoTreeDraw.ViewModel
{
    public class SidePanelViewModel
    {

        public ObservableCollection<Node> Nodes { get; set; }

        public SidePanelViewModel()
        {
            Nodes = new ObservableCollection<Node>() {
                new BST() { X = 0, Y = 0, diameter = 50}
            };

            MouseDownNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownNode);
            MouseMoveNodeCommand = new RelayCommand<MouseEventArgs>(MouseMoveNode);
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
        }

        //Commands

        public ICommand MouseDownNodeCommand { get; }
        public ICommand MouseMoveNodeCommand { get; }
        public ICommand MouseUpNodeCommand { get; }

        private MouseEventCommands mecs { get; set; }

        private void MouseDownNode(MouseButtonEventArgs e)
        {
            mecs.MouseDownNode(e);
        }

        private void MouseUpNode(MouseButtonEventArgs e)
        {
            mecs.MouseUpNode(e);
        }

        private void MouseMoveNode(MouseEventArgs e)
        {
            mecs.MouseMoveNode(e);
        }

    }
}
