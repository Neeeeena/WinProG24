using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Model
{
    public class T234 : Node
    {
        public T234()
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
            return new T234();
        }
    }
}
