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
        public string TextOne { get; set; }
        public string TextTwo { get; set; }
        public string TextThree { get; set; }

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
        }
    

        public override Node NewNode()
        {
            return new T234();
        }

        public T234 Merge(T234 other, T234 optional = null)
        {
            if (IsTwoNode && other.IsTwoNode)
            {
                TextTwo = other.TextOne;
                IsTwoNode = false;
                IsThreeNode = true;
            }else if (IsTwoNode && other.IsThreeNode)
            {
                TextTwo = other.TextOne;
                TextThree = other.TextTwo;
                IsTwoNode = false;
                IsFourNode = true;
            }
            if (optional != null && optional.IsTwoNode && IsThreeNode)
            {
                TextThree = optional.TextOne;
                IsThreeNode = false;
                IsFourNode = true;
            }
            return this;
        }

        public List<T234> Split()
        {
            List<T234> temp = new List<T234>();
            temp.Add(this);
            if (IsThreeNode)
            {  
                temp.Add(new T234() { TextOne = this.TextOne, IsTwoNode = true, ID=IDCounter });
                IDCounter++;
                IsThreeNode = false;
                IsTwoNode = true;
            }
            else if (IsFourNode)
            {
                temp.Add(new T234() { TextOne = this.TextOne, IsTwoNode=true, ID=IDCounter });
                IDCounter++;
                temp.Add(new T234() { TextOne = this.TextTwo, IsTwoNode = true, ID=IDCounter });
                IDCounter++;
                IsFourNode = false;
                IsTwoNode = true;
                
            }
            return temp;
        }


    }
}
