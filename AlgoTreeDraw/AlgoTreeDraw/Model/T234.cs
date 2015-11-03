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
        //public T234()
        //{
        //    initialX = -225;
        //    initialY = 20;
        //}
        Brush color = Brushes.White;
        Brush preColor = Brushes.White;

        public override Brush PreColor
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
            return new T234();
        }
    }
}
