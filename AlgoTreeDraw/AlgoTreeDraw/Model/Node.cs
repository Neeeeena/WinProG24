using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Serialization;

namespace AlgoTreeDraw.Model
{
    [XmlInclude(typeof(BST))]
    [XmlInclude(typeof(RBT))]
    [XmlInclude(typeof(T234))]
    public abstract class Node {


        public _color color;
        public _color preColor;
        public static int IDCounter { get; set; } = 3;
        public Node()
        {
        }

        public struct _color
        {
            public byte R { get; set; }
            public byte B { get; set; }
            public byte G { get; set; }

        }

        private double x = 200;
        private double y = 200;
        public double X { get { return x; } set { x = value; } }
        
        public double Y { get { return y; } set { y = value;  } }

        public double diameter {get; set; } // Tilføj Notify hvis Diameter skal ændres

        private string key = "1";
        public string Key { get { return key; } set { key = value; } }

        public abstract Node NewNode();


        public double CanvasCenterX { get { return X + diameter / 2; } set { X = value - diameter / 2; } }

        public double CanvasCenterY { get { return Y + diameter / 2; } set { Y = value - diameter / 2; } }

        const int LEFT = 0;
        const int RIGHT = 1;

        public int ID { get; set; }       

    }
}
