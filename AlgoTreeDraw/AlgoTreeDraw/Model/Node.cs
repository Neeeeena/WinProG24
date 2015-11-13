using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Diagnostics;

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

        private static int X_OFFSET = 40;
        private static int Y_OFFSET = 50;
        private static int X_ONSET = 28;
        //Use on root
        public bool makePretty()
        {
            Node[] children = getChildren();
            int dir = -1;

            if(children[0] == null)
            {

            }
            else if(children[1] == null)    //One child
            {
                if(children[0].key < key)
                    moveOffset(children[0], LEFT);
                else
                    moveOffset(children[0], RIGHT);
                dir = checkAncestors(children[0]);
                children[0].pushParents(dir);
                //if (ca[0] != -1)
                //{
                //    children[0].x = ca[1];
                //    moveParent(ca[0] == 0);
                //}
                children[0].makePretty();
            }
               //TWO CHILDREN 
            else
            {
                //children[0].x = x - X_OFFSET;
                //children[0].y = y + Y_OFFSET;
                moveOffset(children[LEFT], LEFT);

                dir = checkAncestors(children[0]);
                children[LEFT].pushParents(dir);
                //if (ca[0] != -1)
                //{
                //    children[0].x = ca[1];
                //    moveParent(ca[0] == 0);

                //}
                //children[1].x = x + X_OFFSET;
                //children[1].y = y + Y_OFFSET;
                moveOffset(children[RIGHT], RIGHT);
                dir = checkAncestors(children[1]);
                children[RIGHT].pushParents(dir);
                //if (ca[0] != -1) {
                //    children[1].x = ca[1];
                //    moveParent(ca[0] == 0);
                //}
                children[0].makePretty();
                children[1].makePretty();
            }
            
            return true;
        }

        private int checkAncestors(Node orig)
        {
            Node parent = getParent();
            if (parent == null)
                return NOMOVE;
            if (parent.x < orig.x + X_ONSET &&
                parent.x > orig.x - X_ONSET)
            {
                if (parent.getChildren()[0] == this)
                {
                    orig.moveOnset(LEFT);
                    return LEFT;
                }
                else
                {
                    orig.moveOnset(RIGHT);
                    return RIGHT;
                }
            }
            return parent.checkAncestors(orig);
        }

        private Node getParent()
        {

            foreach (Node neighbour in neighbours)
                if (neighbour.y < y) return neighbour;
            return null;
        }

        //private bool moveParent(bool left)
        //{
        //    Node parent = getParent();
        //    if (parent == null)
        //        return false;
        //    else if (left && parent.x < x)
        //    {
        //        parent.x -= X_ONSET;
        //        parent.moveParent(left);
        //    }
        //    else if (!left && parent.x > x)
        //    {
        //        parent.x += X_ONSET;
        //        parent.moveParent(left);
        //    }
        //    return true;
        //}

        public static int LEFT = 0;
        public static int RIGHT = 1;
        public static int NOMOVE = -1;

        private bool moveOffset(Node child, int direction)
        {
            if (LEFT == direction)
                child.x = x - X_OFFSET;
            else
                child.x = x + X_OFFSET;
            child.y = y + Y_OFFSET;
            return true;
        }

        private bool moveOnset(int direction)
        {
            if (LEFT == direction)
                x = x - X_ONSET;
            else
                x = x + X_ONSET;
            return true;
        }

        private bool pushParents(int direction)
        {
            Debug.WriteLine("Started pushParents()");
            Node parent = getParent();
            if (parent == null)
                return false;
            else if (parent.x < x && direction == LEFT)
            {
                //if (parent.getParent() != null &&
                //    parent.getParent().pushParents(direction) == false)
                //    parent.pushParents(direction);
                parent.moveOnset(LEFT);
                Debug.WriteLine("Parent pushed left");
                //parent.pushParents(LEFT);
                parent.pushChildren();
                return true;
            }

            else if (parent.x > x && direction == RIGHT)
            {
                parent.moveOnset(RIGHT);
                Debug.WriteLine("Parent pushed righty");
                //parent.pushParents(RIGHT);
                parent.pushChildren();
                return true;
            }


            return false;
        }

        private bool pushChildren()
        {
            Node[] children = getChildren();
            
            if(children[RIGHT] != null)
            {
                moveOffset(children[LEFT],LEFT);
                moveOffset(children[RIGHT], RIGHT);
                children[LEFT].pushChildren();
                children[RIGHT].pushChildren();
            }
            else if (children[LEFT] != null)
                if (children[0].key < key)
                    moveOffset(children[0], LEFT);
                else
                    moveOffset(children[0], RIGHT);

            return true;
        }
    }
}
