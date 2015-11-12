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
            color = Brushes.White;
            preColor = Brushes.White;
            borderColor = Brushes.Black;
            borderThickness = 1;
        }
    

        public override Node NewNode()
        {
            return new T234();
        }
    }
}
