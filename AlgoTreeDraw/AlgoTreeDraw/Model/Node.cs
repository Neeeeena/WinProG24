using System;
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
        private int key;
        public string VisualText { get { return visualText; } set { visualText = value; } }
        public int Key
        {
            get { return key; }
            set { key = value; VisualText = "" + key; }
        }

        public abstract Node NewNode();


        public double CanvasCenterX { get { return X + diameter / 2; } set { X = value - diameter / 2; } }

        public double CanvasCenterY { get { return Y + diameter / 2; } set { Y = value - diameter / 2; } }

       
        
        
        public Brush color { get; set; }
        public Brush preColor { get; set; }


        //For adding data structure

        //public Node(int _key)
        //{
        //    key = _key;
        //}

        public LinkedList<Node> neighbours = new LinkedList<Node>();

        //returns true if tree is valid after add
        public bool addNeighbour(Node node)
        {
            neighbours.AddLast(node);
            node.neighbours.AddLast(this);
            return isValid() && node.isValid();
        }

        //returns true if tree is valid after remove
        public bool removeNeighbour(Node node)
        {
            node.neighbours.Remove(this);
            neighbours.Remove(node);
            return isValid() && node.isValid();
        }

        public bool isChild(Node node)
        {
            return node.y > y;
        }

        //Returns true if node has maximum of one parent
        public bool isValid()
        {
            int count = 0;
            foreach (Node neighbour in neighbours)
                if (neighbour.y < y) count++;

            //if (count > 1) return false; return true;
            return count <= 1;
        }


        public LinkedList<Node> getChildren()
        {
            LinkedList<Node> children = new LinkedList<Node>();
            foreach (Node neighbour in neighbours)
                if (neighbour.y > y)
                    children.AddLast(neighbour);
            return children;
        }

        public bool isRoot()
        {
            foreach (Node neighbour in neighbours)
                if (neighbour.y < y) return false;
            return true;
        }

        public Node getRoot()
        {
            foreach (Node neighbour in neighbours)
                if (neighbour.y < y) return neighbour.getRoot();
            return this;
        }

        public int childrenCount()
        {
            int children = 0;
            foreach (Node neighbour in neighbours)
                if (neighbour.y > y) children++;
            return children;
        }

        //USE ON ROOT
        public bool isBST()
        {
            int left = 0;
            int right = 0;
            if (isValid() && childrenCount() <= 2)
            {
                foreach (Node child in getChildren())
                {
                    if (child.x < x) left++; else right++;
                    if (!child.isBST()) return false;
                }
                if (left > 1 || right > 1)
                    return false;
            }
            else
                return false;
            return true;
        }

        //USE ON ROOT //KIG MERE HER (visuelle del)
        public bool autoAddBST(int _key)
        {
            foreach (Node neighbour in neighbours)
            {
                if (neighbour.y > y && neighbour.x < x && _key < key)
                    neighbour.autoAddBST(_key);
                else if (neighbour.y > y && neighbour.x > x && _key > key)
                    neighbour.autoAddBST(_key);

            }

            return true;
        }


    }
}
