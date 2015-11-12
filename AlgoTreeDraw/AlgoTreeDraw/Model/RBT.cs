using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AlgoTreeDraw.Model
{
    public class RBT : Node
    {

        public RBT ()
        {
            color = Brushes.Red;
            preColor = Brushes.Red;
            borderColor = Brushes.Black;
            borderThickness = 1;

        }
       
        public override Node NewNode()
        {
            return new RBT() { X = -145, Y = 20, diameter = 50};
        }
    }
}
