using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlgoTreeDraw.Model
{
    public class BST : Node
    {
        public BST()
        {
            initialX = -225;
            initialY = 20;
        }

        public override Node NewNode()
        {
            return new BST() { X = -225, Y = 20, diameter = 50 };
        }
    }
}
