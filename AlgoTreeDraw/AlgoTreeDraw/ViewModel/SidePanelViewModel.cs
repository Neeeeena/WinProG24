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
    public class SidePanelViewModel : MainViewModelBase
    {
        public new ICommand MouseUpNodeCommand { get; }
        Brush _background;
        public ICommand AddLineCommand { get; }
        public int WIDTH { get; set; } = 240;
        public int NODEHEIGHT { get; set; } = 13;
        public static ObservableCollection<NodeViewModel> NodesSP{ get; set; } 
            = new ObservableCollection<NodeViewModel>
            {
                 new BSTViewModel(new BST() { X = 0, Y = 0, diameter = 50 }),
                 new RBTViewModel(new RBT() { X = 50, Y = 0, diameter = 50 })
            };

        
        public  SidePanelViewModel()
        {
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            AddLineCommand = new RelayCommand<MouseButtonEventArgs>(AddLineClicked);
        }

        private void AddLineClicked(MouseButtonEventArgs e)
        {
            isAddingLine = !isAddingLine;
            RaisePropertyChanged("BackgroundAddLine");
        }

        public new void MouseUpNode(MouseButtonEventArgs e)
        {
            var node = MouseUpNodeSP2(e);
            if(node.X > WIDTH)
            {
                NodeViewModel tempNode = node.newNodeViewModel();
                tempNode.X = node.X - WIDTH;
                tempNode.Y = node.Y + NODEHEIGHT;
                tempNode.Diameter = node.Diameter;
                AddNode(tempNode);

            }
            node.X = initialNodePosition.X;
            node.Y = initialNodePosition.Y;


            e.MouseDevice.Target.ReleaseMouseCapture();


        }


        

        public bool isNodeOutSideSidePanel(NodeViewModel node) {
            return node.X > WIDTH;
        }


        public Brush BackgroundAddLine
        {
            get { return isAddingLine ? Brushes.Pink : Brushes.LightGreen; }
            set { _background = value; }
        }

    }
}
