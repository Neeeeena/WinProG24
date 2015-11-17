using AlgoTreeDraw.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace AlgoTreeDraw.ViewModel
{
    class RBTViewModel : NodeViewModel
    {
        public ICommand SwitchToBlack { get; }
        public ICommand SwitchToRed { get; }
        public RBTViewModel(Node _node) : base(_node) {
            SwitchToBlack = new RelayCommand<MouseButtonEventArgs>(ToBlack);
            SwitchToRed = new RelayCommand<MouseButtonEventArgs>(ToRed);
        }

        public override NodeViewModel newNodeViewModel()
        {
            return new RBTViewModel(new RBT() { diameter = 50 }); //{ Color = Color, PreColor = PreColor };
        }

        public void ToBlack(MouseButtonEventArgs e)
        {
            Color = Brushes.Black;
            PreColor = Brushes.Black;
        }
        public void ToRed(MouseButtonEventArgs e)
        {
            Color = Brushes.Red;
            PreColor = Brushes.Red;
        }
    }
}
