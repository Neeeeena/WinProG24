using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AlgoTreeDraw.Model
{
    public class BST : Node
    {

        public BST()
        {
            color.R = 255;
            color.G = 255;
            color.B = 255;

            preColor.R = 255;
            preColor.G = 255;
            preColor.B = 255;

        }

        public override Node NewNode()
        {
            return new BST() { X = -225, Y = 20, diameter = 50 };
        }

    }
}
