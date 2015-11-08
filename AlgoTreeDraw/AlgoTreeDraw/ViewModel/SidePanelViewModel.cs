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
        public new ICommand MouseLeftButtonUp { get; }
        Brush _background;
        public ICommand AddLineCommand { get; }
        public static int WIDTH { get; set; } = 240;
        public int NODEHEIGHT { get; set; } = 13;
        public static ObservableCollection<NodeViewModel> NodesSP{ get; set; } 
            = new ObservableCollection<NodeViewModel>
            {
                 new BSTViewModel(new BST() { X = WIDTH/3-WIDTH/3+(240-(WIDTH-WIDTH/3+50))/2, Y = 0, diameter = 50 }),
                 new RBTViewModel(new RBT() { X = WIDTH/3*2-WIDTH/3+(240-(WIDTH-WIDTH/3+50))/2, Y = 0, diameter = 50 }),
                 new T234ViewModel(new T234() { X = WIDTH-WIDTH/3+(240-(WIDTH-WIDTH/3+50))/2, Y = 0, diameter = 50 },1)
            };
        
        
        public  SidePanelViewModel()
        {
            MouseLeftButtonUp = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            AddLineCommand = new RelayCommand<MouseButtonEventArgs>(AddLineClicked);
        }

        private void AddLineClicked(MouseButtonEventArgs e)
        {
            isAddingLine = !isAddingLine;
            if(!isAddingLine) fromNode = null;
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
