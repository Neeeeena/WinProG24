using AlgoTreeDraw.Model;
using System;
using GalaSoft.MvvmLight;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AlgoTreeDraw.ViewModel
{
    class NodeViewModel : ViewModelBase
    {
        Boolean isEditing =false;
        public ObservableCollection<BST> BSTNodes {get; set; }
        public ObservableCollection<RBT> RBTNodes { get; set; }
        public NodeViewModel()
        {
            BSTNodes = new ObservableCollection<BST>() {
                new BST() { X = 30, Y = 40, diameter = 50},
                new BST() { X = 300, Y = 100, diameter = 50}
            };

            RBTNodes = new ObservableCollection<RBT>()
            {
                new RBT() {X = 60, Y=90, diameter=50 },
                new RBT() {X= 120, Y=50,diameter=50 }
            };
            
        }

        public void edit()
        {
            isEditing = true;
        }
    }
}