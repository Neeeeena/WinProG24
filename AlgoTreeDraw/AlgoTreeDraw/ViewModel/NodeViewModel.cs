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
using System.Diagnostics;

namespace AlgoTreeDraw.ViewModel
{
    public class NodeViewModel : MainViewModelBase
    {

        public NodeViewModel(Node node)
        {
            _node = node;

        }

        //View databinds to the following
        Node _node;

        public Node Node
        {
            get
            {
                return _node;
            }
            set
            {
                _node = value;
            }
        }

        public double X
        {
            get { return Node.X; }
            set { Node.X = value; RaisePropertyChanged(); RaisePropertyChanged(() => CanvasCenterX); }
        }

        public double initialX
        {
            get { return Node.initialX; }
            set { Node.initialX = (int)value; }
        }

        public double Y
        {
            get { return Node.Y; }
            set { Node.Y = value; RaisePropertyChanged(); RaisePropertyChanged(() => CanvasCenterY); }
        }

        public double initialY
        {
            get { return Node.initialY; }
            set { Node.initialY = (int)value; }
        }

        public double CanvasCenterX { get { return X + Diameter / 2; } set { X = value - Diameter / 2; RaisePropertyChanged(() => X); } }
        public double CanvasCenterY { get { return Y + Diameter / 2; } set { Y = value - Diameter / 2; RaisePropertyChanged(() => Y); } }

        public double Diameter
        {
            get { return Node.diameter; }
            set { Node.diameter = value; }
        }

        public string VisualText
        {
            get { return Node.VisualText; }
            set { Node.VisualText = value; }
        }

        public Brush Color
        {
            get { return Node.Color; }
            set { Node.Color = value; }
        }

        public Brush PreColor
        {
            get { return Node.PreColor; }
            set { Node.PreColor = value; }
        }
    }
}
