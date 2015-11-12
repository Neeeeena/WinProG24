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

       
        private Brush _color;
        private Brush _preColor;


        public Brush borderColor { get; set; }
        
        public double borderThickness { get; set; }
        
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

        //Move this to BST only.
        public Node[] getChildren()
        {
            Node[] children = new Node[2];
            int i = 0;
            foreach (Node neighbour in neighbours)
                if (neighbour.y > y)
                {
                    children[i] = neighbour;
                    i++;
                }

            if(i == 2 && children[0].x > children[1].x)
            {
                Node temp = children[0];
                children[0] = children[1];
                children[1] = temp;
            }    
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
        public bool isValidBST()
        {
            Node[] children = getChildren();
            if (isValid() && childrenCount() <= 2)
            {
                foreach (Node child in children)
                    if (child != null)
                        if (!child.isValidBST()) return false;
                if(children[1] != null) return children[0].key < children[1].key;
            }
            else return false;
            return true;
        }

        //use on valid bst
        //USE ON ROOT //KIG MERE HER (visuelle del)
        public Node insertBST(Node newnode)
        {
            Node[] children = getChildren();


            if (children[0] == null)
            {
                addNeighbour(newnode);
                return this;
            }
            else if (children[1] == null)
                if ((key < newnode.key && children[0].key < newnode.key) ||
                    (key >= newnode.key && children[0].key >= newnode.key))
                    return children[0].insertBST(newnode);
                else
                {
                    addNeighbour(newnode);
                    return this;
                }
                   
            else if (key < newnode.key)
                return children[0].insertBST(newnode);
            else
                return children[1].insertBST(newnode);
        }




    }
}
