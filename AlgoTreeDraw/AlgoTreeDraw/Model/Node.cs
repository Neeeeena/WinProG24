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
        public static int IDCounter { get; set; }
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

        [XmlIgnore]
        public List<Node> neighbours = new List<Node>();

        //For adding data structure

        //public Node(int _key)
        //{
        //    key = _key;
        //}
        //Hvad fuck er det her?
        //[XmlIgnore]

        //returns true if tree is valid after add
        //public bool addNeighbour(Node node)
        //{
        //    neighbours.Add(node);
        //    node.neighbours.Add(this);
        //    return isValid() && node.isValid();
        //}

        //returns true if tree is valid after remove
        //public bool removeNeighbour(Node node)
        //{
        //    node.neighbours.Remove(this);
        //    neighbours.Remove(node);
        //    return isValid() && node.isValid();
        //}

        //public bool isChild(Node node)
        //{
        //    return node.y > y;
        //}

        //Returns true if node has maximum of one parent
        public bool isValid()
        {
            int pCount = 0; //parent(s)
            int nCount = 0; //neighbour(s)
            foreach (Node neighbour in neighbours)
            {
                if (neighbour.Y < Y) pCount++;
                nCount++;
            }
            return pCount <= 1 && nCount - pCount < 3;
        }

        //Move this to BST only.
        //public Node[] getChildren()
        //{
        //    Node[] children = new Node[3];
        //    int i = 0;
        //    foreach (Node neighbour in neighbours)
        //        if (neighbour.y > y)
        //        {
        //            children[i] = neighbour;
        //            i++;
        //        }

        //    if(i == 2 && children[0].x > children[1].x)
        //    {
        //        Node temp = children[0];
        //        children[0] = children[1];
        //        children[1] = temp;
        //    }    
        //    return children;
        //}

        //public bool isRoot()
        //{
        //    foreach (Node neighbour in neighbours)
        //        if (neighbour.y < y) return false;
        //    return true;
        //}

        //public Node getRoot()
        //{
        //    foreach (Node neighbour in neighbours)
        //        if (neighbour.y < y) return neighbour.getRoot();
        //    return this;
        //}

        //public int childrenCount()
        //{
        //    int children = 0;
        //    foreach (Node neighbour in neighbours)
        //        if (neighbour.y > y) children++;
        //    return children;
        //}

        //USE ON ROOT
        ////////////////////public bool isValidBST()
        ////////////////////{
        ////////////////////    Node[] children = getChildren();
        ////////////////////    if (isValid() && childrenCount() <= 2)
        ////////////////////    {
        ////////////////////        foreach (Node child in children)
        ////////////////////            if (child != null)
        ////////////////////                if (!child.isValidBST()) return false;
        ////////////////////        if(children[RIGHT] != null) return children[LEFT].key <= children[RIGHT].key;
        ////////////////////    }
        ////////////////////    else return false;
        ////////////////////    return true;
        ////////////////////}
        // HER MATHIAS
        //public bool isBST()
        //{
        //    Node[] children = getChildren();
        //    if (isValid() && childrenCount() <= 2)
        //    {
        //        foreach (Node child in children)
        //            if (child != null)
        //                if (!child.isBST()) return false;
        //    }
        //    else return false;
        //    return true;
        //}

        //use on valid bst
        //USE ON ROOT //KIG MERE HER (visuelle del)
        //////////////public Node insertBST(Node newnode)
        //////////////{
        //////////////    Node[] children = getChildren();


        //////////////    if (children[0] == null)
        //////////////    {
        //////////////        addNeighbour(newnode);
        //////////////        return this;
        //////////////    }
        //////////////    else if (children[1] == null)
        //////////////        if ((key < newnode.key && children[0].key < newnode.key) ||
        //////////////            (key >= newnode.key && children[0].key >= newnode.key))
        //////////////            return children[0].insertBST(newnode);
        //////////////        else
        //////////////        {
        //////////////            addNeighbour(newnode);
        //////////////            return this;
        //////////////        }

        //////////////    else if (key < newnode.key)
        //////////////        return children[0].insertBST(newnode);
        //////////////    else
        //////////////        return children[1].insertBST(newnode);
        //////////////}



        //private static int X_OFFSET = 40;
        //private static int Y_OFFSET = 50;
        //private static int X_ONSET = 28;
        //private bool isLeftChild = false;
        //private Node[] childrenFromList;
        //private static int NOMOVE = -1;
        //private static int ONLY = 0;
        //private static int LEFT = 0;
        //private static int RIGHT = 1;
        //private static int NONE = -1;
        //private List<Node> allNodes = new List<Node>();

        //public Node[] IsLeftChild { get { return childrenFromList; } }
        //public List<Node> AllNodes { get { return allNodes; } }
        //Use on root



        //public bool makePretty()
        //{
        //    updateList(); //Updating the nodes in allNodes, to run the tree through breadth-first
        //    if (!isValidBST())
        //        return false;

        //    foreach (Node node in allNodes)
        //    { 

        //        if (node.childrenFromList[0] == null) //IF THERE IS NO CHILDREN
        //        {

        //        }
        //        else if (node.childrenFromList[RIGHT] == null)    //One child
        //        {
        //            if (node.childrenFromList[ONLY].isLeftChild)
        //                node.moveOffset(node.childrenFromList[ONLY], LEFT);
        //            else
        //                node.moveOffset(node.childrenFromList[ONLY], RIGHT);
        //            node.pushAncenstors(node.childrenFromList[ONLY]);
        //        }
        //        else
        //        {
        //            node.moveOffset(node.childrenFromList[LEFT], LEFT);
        //            node.moveOffset(node.childrenFromList[RIGHT], RIGHT);

        //            node.pushAncenstors(node.childrenFromList[LEFT]);
        //            node.pushAncenstors(node.childrenFromList[RIGHT]);
        //        }      
        //    }
        //    return true;
        //}

        //private bool pushAncenstors(Node orig)
        //{
        //    Node parent = getParent();
        //    if (parent == null)
        //    {
        //        return false;
        //    }

        //    if (parent.x + X_ONSET-1 > orig.x && parent.x - X_ONSET+1 < orig.x)
        //    {
        //        if(x < parent.x)
        //        {
        //            getRoot().pushTree(LEFT, parent.x, orig, X_ONSET + (orig.x - parent.x) );                   
        //            orig.move(LEFT, X_ONSET + (orig.x - parent.x));                    
        //        }
        //        else
        //        {
        //            getRoot().pushTree(RIGHT, parent.x, orig, X_ONSET + (parent.x - orig.x) );
        //            orig.move(RIGHT, X_ONSET + (parent.x - orig.x) );                  
        //        }

        //    }
        //    return parent.pushAncenstors(orig);
        //}

        //private bool pushTree(int direction, double threshold, Node orig, double offset)
        //{
        //    Node[] children = getChildren();
        //    if(this == orig)
        //        return true;
        //    if (direction == LEFT && x < threshold)
        //        move(LEFT, offset);
        //    else
        //    if (direction == RIGHT && threshold < x)
        //        move(RIGHT, offset);

        //    int i = 0;
        //    while (children[i] != null)
        //    {
        //        children[i].pushTree(direction, threshold, orig, offset);
        //        i++;
        //    }
        //    return true;
        //}

        //private bool isSingleChildLeft()
        //{
        //    Node parent = getParent();
        //    if (parent == null)
        //        return false;
        //    else if (parent.x < x)
        //        return false;
        //    else
        //        return true;
        //}

        //private Node getParent()
        //{
        //    foreach (Node neighbour in neighbours)
        //        if (neighbour.y < y) return neighbour;
        //    return null;
        //}



        //private bool moveOffset(Node child, int direction)
        //{
        //    if (LEFT == direction)
        //        child.X = x - X_OFFSET;
        //    else
        //        child.X = x + X_OFFSET;
        //    child.Y = y + Y_OFFSET;
        //    return true;
        //}

        //private bool move(int direction, double offset)
        //{
        //    if (LEFT == direction)
        //        X = x - offset;
        //    else
        //        X = x + offset;
        //    return true;
        //}

        //private static LinkedList<Node> queue = new LinkedList<Node>();
        //private void updateList()
        //{
        //    allNodes = new List<Node>();
        //    int i = 0;
        //    Node node = getRoot();
        //    queue.Clear();
        //    queue.AddLast(getRoot());
        //    queue.First();

        //    for (;;)
        //    {
        //        node.childrenFromList = node.getChildren();
        //        allNodes.Add(node);
        //        i = 0;
        //        foreach(Node child in node.childrenFromList) 
        //            if(child != null)
        //            {
        //                queue.AddLast(node.childrenFromList[i]);
        //                i++;
        //            }
        //        if(i==1)
        //            node.childrenFromList[LEFT].isLeftChild = node.childrenFromList[LEFT].isSingleChildLeft();

        //        if (queue.Count == 1)
        //            break;
        //        queue.RemoveFirst();
        //        node = queue.First();
        //    }

        //}

    }
}
