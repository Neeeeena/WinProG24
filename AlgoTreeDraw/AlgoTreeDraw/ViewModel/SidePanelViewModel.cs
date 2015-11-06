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
        public int WIDTH { get; set; } = 240;
        public static ObservableCollection<NodeViewModel> NodesSP{ get; set; } 
            = new ObservableCollection<NodeViewModel>
            {
                 new BSTViewModel(new BST() { X = 0, Y = 0, diameter = 50 }),
                 new RBTViewModel(new RBT() { X = 50, Y = 0, diameter = 50 }),
                 new T234ViewModel(new T234() { X = 100, Y = 0, diameter = 50 })
            };

        
        public  SidePanelViewModel()
        {
            MouseUpNodeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
        }

        public new void MouseUpNode(MouseButtonEventArgs e)
        {
            var node = MouseUpNodeSP2(e);
            if(node.X > WIDTH)
            {
                
                if(node is BSTViewModel)
                {
                    AddNode(new BSTViewModel(new BST() { X = node.X - WIDTH, Y = node.Y , diameter = node.Diameter }));
                } else if(node is RBTViewModel)
                {
                    AddNode(new RBTViewModel(new RBT() { X = node.X - WIDTH, Y = node.Y, diameter = node.Diameter }));
                }
            }
            node.X = initialNodePosition.X;
            node.Y = initialNodePosition.Y;


            e.MouseDevice.Target.ReleaseMouseCapture();


        }



        Brush _background;
        public ICommand AddLineCommand { get;  }


   
        

        public bool isNodeOutSideSidePanel(NodeViewModel node) {
            return node.X > WIDTH;
        }


        public Brush BackgroundAddLine
        {
            get { return isAddingLine ? Brushes.Pink : Brushes.LightGreen; }
            set { _background = value; }
        }


        private void AddLineClicked()
        {
            isAddingLine = !isAddingLine;
            RaisePropertyChanged("BackgroundAddLine");
        }
    }
}
