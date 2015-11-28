using AlgoTreeDraw.Command;
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
        
        
        public ICommand AddLineCommand { get; }
        public ICommand SelectCommand { get; }

        public ICommand ChangeColor { get; }
        public ICommand ChangeColorOfText { get; }


        public ICommand MakePrettyCommand { get; }

        public ICommand AutoBalanceCommand { get; }

        public ICommand InsertNodeCommand { get; }

        
        //sidepanel WIDTHS
        public static int WIDTHS { get; set; } = 150;

        public string AddNodeValue { get; set; }

        public ObservableCollection<BSTViewModel> BST { get; set; }
        = new ObservableCollection<BSTViewModel>
        {
            new BSTViewModel(new BST() { X = 20, Y = 20, diameter = 50, ID=0})
        };
        public ObservableCollection<RBTViewModel> RBT { get; set; }
        = new ObservableCollection<RBTViewModel>
        {
            new RBTViewModel(new RBT() { X = 20, Y = 95, diameter = 50, ID=1 })
        };
        public ObservableCollection<T234ViewModel> T234 { get; set; }
        = new ObservableCollection<T234ViewModel>
        {
            new T234ViewModel(new T234() { X = 15, Y = 170, diameter = 30, IsThreeNode=true, ID=2 })
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
            InsertNodeCommand = new RelayCommand(CallInsertNode);
            ChosenColor = Color.FromRgb(0,0,0);


        }

        private void CallInsertNode()
        {
            int key = 0;
            if(int.TryParse(AddNodeValue, out key))
            {
                NodeViewModel nvm = new BSTViewModel(new BST() { X = 20, Y = 20, Key = key.ToString(), ID = Node.IDCounter });
                Node.IDCounter++;
                nvm.Diameter = 50;
                undoRedo.InsertInUndoRedo(new InsertNodeInTreeCommand(Nodes, selectedNodes, nvm, Lines));
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid Key");
            }
            
        }
        private void CallAutoBalance()
        {
            if (selectedNodes.Count != 0)
            {
                if (selectedNodes.ElementAt(0) is BSTViewModel)
                {
                    undoRedo.InsertInUndoRedo(new AutoBalanceCommand(Nodes, selectedNodes, Lines));
                }
                else if (selectedNodes.ElementAt(0) is T234ViewModel)
                {
                    List<NodeViewModel> nodesCopy = selectedNodes;
                    foreach (var n in selectedNodes)
                    {
                        Nodes.Remove(n);
                    }
                    Tree234 tree = new Tree234(nodesCopy);
                    Tuple<List<LineViewModel>, List<NodeViewModel>> tuple = tree.BalanceT234();
                    Nodes.AddRange(tuple.Item2);
                    Lines.AddRange(tuple.Item1);
                }
            }
            
        }
        private void CallMakePretty()
        {
            undoRedo.InsertInUndoRedo(new MakePrettyCommand(Nodes, selectedNodes));
        }
        private void Select()
        {
            isMarking = !isMarking;
            //Reset
        }

        private void AddLineClicked(MouseButtonEventArgs e)
        {
            isAddingLine = !isAddingLine;
            if(isAddingLine)
            {
                Messenger.Default.Send(Cursors.Cross);
            } else
            {
                Messenger.Default.Send(Cursors.Arrow);
            }
            if(!isAddingLine) fromNode = null;

        }

        public void MouseUpNode(MouseButtonEventArgs e)
        {
            var node = MouseUpNodeSP2(e);

            if(node.X > 120)
            {
                NodeViewModel tempNode = node.newNodeViewModel();
                tempNode.X = (node.X - WIDTHS+27)/zoomValue;
                tempNode.Y = (node.Y + 31)/zoomValue;
                Debug.Write("Zoom: " + zoomValue);
                tempNode.ID = Node.IDCounter;
                Node.IDCounter++;
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
            if(isChangingColor)
            {
                Messenger.Default.Send(Cursors.Pen);
            } else
            {
                Messenger.Default.Send(Cursors.Arrow);
            }
        }

        private void ChangeColorTextClicked()
        {
            isChangingColorText = !isChangingColorText;
            if (isChangingColorText)
            {
                Messenger.Default.Send(Cursors.Pen);
            }
            else
            {
                Messenger.Default.Send(Cursors.Arrow);
            }
        }


        public bool isNodeOutSideSidePanel(NodeViewModel node) {
            return node.X > WIDTHS;
        }




    }
}
