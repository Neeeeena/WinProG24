using System;
using System.Collections.Generic;
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
        public override Node NewNode()
        {
            return new T234();
        }
    }
}
