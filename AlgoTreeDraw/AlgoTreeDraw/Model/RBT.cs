using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Model
{
    public class RBT : Node
    {

        public override Node NewNode()
        {
            return new RBT() { diameter = 50};
        }
    }
}
