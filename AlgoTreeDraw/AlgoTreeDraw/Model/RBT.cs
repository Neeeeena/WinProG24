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

        initialX = -145;
        initialY = 20;

       }
        Brush color = Brushes.Red;
        Brush preColor = Brushes.Red;

        public override System.Windows.Media.Brush PreColor
        {
            get
            {
                return preColor;
            }
        }

        public override Brush Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value; NotifyPropertyChanged(); NotifyPropertyChanged(() => Color);
            }
        }
        public override Node NewNode()
        {
            return new RBT() { X = -145, Y = 20, diameter = 50};
        }
    }
}
