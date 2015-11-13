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


        public Line Line { get; set; }

        public NodeViewModel To
        {
            get { return to; }
            set { to = value; RaisePropertyChanged(); }
        }
        public NodeViewModel From
        {
            get { return from; }
            set { from = value; RaisePropertyChanged(); }
        }
    }
}

