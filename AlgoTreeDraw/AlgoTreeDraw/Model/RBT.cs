using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Model
{
    public class RBT : Node
    {

        public RBT ()
        {

        initialX = -145;
        initialY = 20;

       }

        public override Node NewNode()
        {
            return new RBT() { X = -145, Y = 20, diameter = 50};
        }
    }
}
