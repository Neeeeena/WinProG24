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

        public LineViewModel(Line line)
        {
            _line = line;
        }

        //Commands

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

