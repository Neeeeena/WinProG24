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
    public class LineViewModel : MainViewModelBase
    {
        NodeViewModel to;
        NodeViewModel from;
        Line _line;


        public LineViewModel(Line line)
        {
            _line = line;
        }



        public Line Line { get { return _line; } set { _line = value; } }

        public NodeViewModel To
        {
            get { return to; }
            set { to = value; Line.To = value.Node;  RaisePropertyChanged(); }
        }
        public NodeViewModel From
        {
            get { return from; }
            set { from = value; Line.From = value.Node; RaisePropertyChanged(); }
        }
    }
}

