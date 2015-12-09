using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Model
{
    public class T234 : Node
    {
        public bool IsTwoNode { get; set; }
        public bool IsThreeNode { get; set; }
        public bool IsFourNode { get; set; }
        public string TextTwo { get; set; }
        public string TextThree { get; set; }

        public string PreTextTwo { get; set; }
        public string PreTextThree { get; set; }

        public T234()
        {
            color.R = 255;
            color.G = 255;
            color.B = 255;

            preColor.R = 255;
            preColor.G = 255;
            preColor.B = 255;

            TextOne = "1";
            TextTwo = "1";
            TextThree = "1";
            diameter = 30;
        }
    

        public override Node NewNode()
        {
            return new T234();
        }




    }
}
