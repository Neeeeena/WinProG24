﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AlgoTreeDraw.Model
{
    public abstract class Node {

        private int x = 200;
        private int y = 200;
        public double X { get { return x; } set { x = (int)value; } }
        public double Y { get { return y; } set { y = (int)value;  } }

        public double diameter {get; set; } // Tilføj Notify hvis Diameter skal ændres

        private string visualText = "1";
        public string VisualText { get { return visualText; } set { visualText = value; } }

        public abstract Node NewNode();


        public double CanvasCenterX { get { return X + diameter / 2; } set { X = value - diameter / 2; } }

        public double CanvasCenterY { get { return Y + diameter / 2; } set { Y = value - diameter / 2; } }


        private Brush _color;
        private Brush _preColor;
        
        public Brush color { get { return _color; } set { _color = value; } }
        public Brush preColor { get { return _preColor; } set { _preColor = value; } }
    }
}
