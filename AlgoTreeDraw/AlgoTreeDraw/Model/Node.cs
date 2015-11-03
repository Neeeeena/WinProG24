using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AlgoTreeDraw.Model
{
    public abstract class Node : NotifyBase {
        public int initialX;
        public int initialY;
        private int x = 200;
        private int y = 200;
        public double X { get { return x; } set { x = (int)value; NotifyPropertyChanged(); NotifyPropertyChanged(() => CanvasCenterX); } }
        public double Y { get { return y; } set { y = (int)value; NotifyPropertyChanged(); NotifyPropertyChanged(() => CanvasCenterY); } }

        public double diameter {get; set; } // Tilføj Notify hvis Diameter skal ændres

        private string visualText = "1";
        public string VisualText { get { return visualText; } set { visualText = value; NotifyPropertyChanged(); } }

        public abstract Node NewNode();


        public double CanvasCenterX { get { return X + diameter / 2; } set { X = value - diameter / 2; NotifyPropertyChanged(() => X); } }

        public double CanvasCenterY { get { return Y + diameter / 2; } set { Y = value - diameter / 2; NotifyPropertyChanged(() => Y); } }

        public abstract Brush PreColor { get; }
        Brush preColor;
        Brush color;
        public abstract Brush Color { get; set; }
    }
}
