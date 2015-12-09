using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AlgoTreeDraw.Model
{

    public class Line
    {

        private Node from;
        public Node From { get { return from; } set { from = value;  } }

        private Node to;
        public Node To { get { return to; } set { to = value;  } }
    }
}
