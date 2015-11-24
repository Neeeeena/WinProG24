using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.ViewModel
{
    public abstract class ThreeViewModel : MainViewModelBase
    {



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
    }
}
