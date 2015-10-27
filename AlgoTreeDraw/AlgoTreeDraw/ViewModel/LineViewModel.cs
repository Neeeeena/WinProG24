using AlgoTreeDraw.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.ViewModel
{
    public class LineViewModel
    {
        public ObservableCollection<Line> Lines { get; set; }
        public LineViewModel()
        {
            Lines = new ObservableCollection<Line>() {
                new Line() {From = new BST() { X = 300, Y = 40, diameter = 50}, To = new BST() { X = 30, Y = 40, diameter = 50}},
                 new Line() {From = new BST() { X = 100, Y = 100, diameter = 50}, To = new BST() { X = 0, Y = 0, diameter = 50}}
            };

        }
    }
}
