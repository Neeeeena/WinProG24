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
            initialX = -225;
            initialY = 20;
        }

        Brush color = Brushes.White;
        Brush preColor = Brushes.White;

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
            return new BST() { X = -225, Y = 20, diameter = 50 };
        }
    }
}
