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
        public NodeViewModel nvm;
        public LineViewModel(NodeViewModel nvm)
        {
            this.nvm = nvm;

            Lines = new ObservableCollection<Line>() {
                new Line() {From = nvm.BSTNodes[0], To = nvm.BSTNodes[1]}
            };

        }
    }
}
