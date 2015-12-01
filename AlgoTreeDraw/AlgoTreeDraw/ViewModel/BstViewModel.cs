using AlgoTreeDraw.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AlgoTreeDraw.ViewModel
{
    public class BSTViewModel : NodeViewModel
    {
        public BSTViewModel(Node _node) : base(_node) {
            ColorOfText = Brushes.Black;
        }



        public override NodeViewModel newNodeViewModel()
        {
            
            return new BSTViewModel(new BST() {X = this.X, Y = this.Y, diameter = this.Diameter,Key=Key }) { Color = Color, PreColor = PreColor, ColorOfText = ColorOfText, PreColorOfText = PreColorOfText };
        }



    }
}
