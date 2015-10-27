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
    class LineViewModel
    {
        public ObservableCollection<Line> Lines { get; set; }
        public LineViewModel()
        {
            Lines = new ObservableCollection<Line>() {
            };

        }
    }
}
