using AlgoTreeDraw.Model;
using System;
using GalaSoft.MvvmLight;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.CommandWpf;
using AlgoTreeDraw.Command;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;

namespace AlgoTreeDraw.ViewModel
{
    public class MainViewModelBase : ViewModelBase
    {
        public ObservableCollection<Node> Nodes { get; set; }
        public ObservableCollection<Line> Lines { get; set; }
        public bool isAddingLine = false;

        public Node fromNode = null;

        public MainViewModelBase()
        {
            Nodes = new ObservableCollection<Node>() {
                new RBT() { X = -145, Y = 20, diameter = 50},
                new BST() { X = -225, Y = 20, diameter = 50 },
            }; Lines = new ObservableCollection<Line>();
        }

        public void AddLine( Node to)
        {
            fromNode.Color = fromNode.PreColor;
            isAddingLine = false;
            Lines.Add(new Line() { From=fromNode, To=to});

        }

        public void AddNode(Node node)
        {
            Node newNode = node.NewNode();
            Nodes.Add(newNode);

        }
    }
}
