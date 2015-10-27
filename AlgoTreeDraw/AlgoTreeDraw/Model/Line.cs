using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Model
{
    public class Line : NotifyBase
    {

        private Node from;
        public Node From { get { return from; } set { from = value; NotifyPropertyChanged(); } }

        private Node to;
        public Node To { get { return to; } set { to = value; NotifyPropertyChanged(); } }
    }
}
