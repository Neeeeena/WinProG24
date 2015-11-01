using AlgoTreeDraw.Model;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgoTreeDraw.ViewModel
{
    public class LineViewModel
    {
        public ObservableCollection<Line> Lines { get; set; }
        public NodeViewModel nvm;
        public LineViewModel()
        {
            Messenger.Default.Register<NodeMessage>(this, (action) => ReceiveMessage(action)); //Register message from BstViewModel
            Lines = new ObservableCollection<Line>() {
                new Line() {From = new BST() { X = 500, Y = 500, diameter = 50}, To = new BST() { X = 10, Y = 50, diameter = 50}},
                new Line() {From = new BST() { X = 900, Y = 20, diameter = 50}, To = new BST() { X = 10, Y = 50, diameter = 50}}
            };
        }

        //Commands

        private object ReceiveMessage(NodeMessage action)
        {
            Node node = action.node;
            Lines.Add(new Line() { From = new BST() { X = 0, Y = 0, diameter = 0 }, To = node });
            return null;
        }

        Line _line;

        public Line Line
        {
            get
            {
                return _line;
            }
            set
            {
                _line = value;
            }
        }

        public Node To
        {
            get { return Line.To; }
            set { Line.To = value; }
        }
        public Node From
        {
            get { return Line.From; }
            set { Line.From = value; }
        }

    }

}

