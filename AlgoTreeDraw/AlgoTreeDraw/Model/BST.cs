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

        }

        public override Node NewNode()
        {
            return new BST() { diameter = 50 };
        }
    }
}
