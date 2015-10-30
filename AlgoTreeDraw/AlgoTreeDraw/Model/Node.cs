using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AlgoTreeDraw.Model
{
    public abstract class Node : NotifyBase {
        private double x = 200;
        private double y = 200;
        public double X { get { return x; } set { x = value; NotifyPropertyChanged(); NotifyPropertyChanged(() => CanvasCenterX); } }
        public double Y { get { return y; } set { y = value; NotifyPropertyChanged(); NotifyPropertyChanged(() => CanvasCenterY); } }

        public double diameter {get; set; } // Tilføj Notify hvis Diameter skal ændres

        private string visualText = "1";
        public string VisualText { get { return visualText; } set { visualText = value; NotifyPropertyChanged(); } }

        public abstract Node NewNode();


        public double CanvasCenterX { get { return X + diameter / 2; } set { X = value - diameter / 2; NotifyPropertyChanged(() => X); } }

        public double CanvasCenterY { get { return Y + diameter / 2; } set { Y = value - diameter / 2; NotifyPropertyChanged(() => Y); } }
    }
}
