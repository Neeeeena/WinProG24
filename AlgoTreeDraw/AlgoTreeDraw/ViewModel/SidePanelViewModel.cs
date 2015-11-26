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
        public ICommand ChangeColorOfText { get; }


        public ICommand MakePrettyCommand { get; }

        public ICommand AutoBalanceCommand { get; }

        public int NODEHEIGHT { get; set; } = 13;
        //sidepanel WIDTHS
        public static int WIDTHS { get; set; } = 240;


        public static ObservableCollection<NodeViewModel> NodesSP{ get; set; } 
            = new ObservableCollection<NodeViewModel>
            {
                 new BSTViewModel(new BST() { X = WIDTHS/3-WIDTHS/3+(240-(WIDTHS-WIDTHS/3+50))/2, Y = 0, diameter = 50 }),
                 new RBTViewModel(new RBT() { X = WIDTHS/3*2-WIDTHS/3+(240-(WIDTHS-WIDTHS/3+50))/2-15, Y = 0, diameter = 50 }),
                 //hvis man sætter y = 10, ser det væsentligt bedre ud, men så hopper de når man dragger..
                 new T234ViewModel(new T234() { X = WIDTHS-WIDTHS/3+(240-(WIDTHS-WIDTHS/3+50))/2-30, Y = 0, diameter = 30, IsTwoNode=true })
            };
        
        public  SidePanelViewModel()
        {
            MouseLeftButtonUp = new RelayCommand<MouseButtonEventArgs>(MouseUpNode);
            AddLineCommand = new RelayCommand<MouseButtonEventArgs>(AddLineClicked);
            SelectCommand = new RelayCommand(Select);
            ChangeColor = new RelayCommand(ChangeColorClicked);
            ChangeColorOfText = new RelayCommand(ChangeColorTextClicked);
            MakePrettyCommand = new RelayCommand(CallMakePretty);
            AutoBalanceCommand = new RelayCommand(CallAutoBalance);
            ChosenColor = Color.FromRgb(0,0,0);
        }

        private void CallAutoBalance()
        {
            autoBalance();
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

        public void MouseUpNode(MouseButtonEventArgs e)
        {
            var node = MouseUpNodeSP2(e);

            if(node.X > WIDTHS)
            {
                NodeViewModel tempNode = node.newNodeViewModel();
                tempNode.X = node.X - WIDTHS;
                tempNode.Y = node.Y + NODEHEIGHT;
                tempNode.Key = node.ID.ToString();
                AddNode(tempNode);
            }
            node.X = node.initialNodePosition.X;
            node.Y = node.initialNodePosition.Y;
            node.BorderColor = Brushes.Black;
            node.BorderThickness = 1;
            selectedNodes.Remove(node);
            node.ID++;
            node.Key = node.ID.ToString();   
        }

        private void ChangeColorClicked()
        {
            isChangingColor = !isChangingColor;
        }

        private void ChangeColorTextClicked()
        {
            isChangingColorText = !isChangingColorText;
        }


        public bool isNodeOutSideSidePanel(NodeViewModel node) {
            return node.X > WIDTHS;
        }


        public Brush BackgroundAddLine
        {
            get { return isAddingLine ? Brushes.Pink : Brushes.LightGreen; }
            set { _background = value; }
        }

    }
}
