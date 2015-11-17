using AlgoTreeDraw.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace AlgoTreeDraw.ViewModel
{
    public class SidePanelViewModel : MainViewModelBase
    {
        public new ICommand MouseLeftButtonUp { get; }
        Brush _background;
        
        
        public ICommand AddLineCommand { get; }
        public ICommand SelectCommand { get; }

        public ICommand ChangeColor { get; }

        public ICommand MakePrettyCommand { get; }

        public int NODEHEIGHT { get; set; } = 13;
        //sidepanel width
        public static int WIDTH { get; set; } = 240;


        public static ObservableCollection<NodeViewModel> NodesSP{ get; set; } 
            = new ObservableCollection<NodeViewModel>
            {
                 new BSTViewModel(new BST() { X = WIDTH/3-WIDTH/3+(240-(WIDTH-WIDTH/3+50))/2, Y = 0, diameter = 50 }),
                 new RBTViewModel(new RBT() { X = WIDTH/3*2-WIDTH/3+(240-(WIDTH-WIDTH/3+50))/2-15, Y = 0, diameter = 50 }),
                 //hvis man sætter y = 10, ser det væsentligt bedre ud, men så hopper de når man dragger..
                 new T234ViewModel(new T234() { X = WIDTH-WIDTH/3+(240-(WIDTH-WIDTH/3+50))/2-30, Y = 10, diameter = 30 },1)
            };
        
        public  SidePanelViewModel()
        {
            MouseLeftButtonUp = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            AddLineCommand = new RelayCommand<MouseButtonEventArgs>(AddLineClicked);
            SelectCommand = new RelayCommand(Select);
            ChangeColor = new RelayCommand(ChangeColorClicked);
            MakePrettyCommand = new RelayCommand(CallMakePretty);
            ChosenColor = Color.FromRgb(0,0,0);
        }

        private void CallMakePretty()
        {
            makePretty();
        }
        private void Select()
        {
            isMarking = !isMarking;
            //Reset
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
                tempNode.X = node.X - WIDTH-2;
                tempNode.Y = node.Y + node.Offset;
                AddNode(tempNode);
            }
            node.X = node.initialNodePosition.X;
            node.Y = node.initialNodePosition.Y;
            node.BorderColor = Brushes.Black;
            node.BorderThickness = 1;
            selectedNodes.Remove(node);            
            
        }

        private void ChangeColorClicked()
        {
            isChangingColor = !isChangingColor;
            Debug.Write(ChosenColor.ToString());
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
