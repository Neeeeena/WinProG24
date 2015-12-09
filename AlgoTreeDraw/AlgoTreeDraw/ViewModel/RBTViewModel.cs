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
    public class RBTViewModel : NodeViewModel
    {
        public ICommand SwitchToBlack { get; }
        public ICommand SwitchToRed { get; }

        public RBTViewModel(Node _node) : base(_node) {
            SwitchToBlack = new RelayCommand<MouseButtonEventArgs>(ToBlack);
            SwitchToRed = new RelayCommand<MouseButtonEventArgs>(ToRed);
            _ColorOfText = Brushes.White;
        }

        public override NodeViewModel newNodeViewModel()
        {

            return new RBTViewModel(new RBT() { X = this.X, Y = this.Y, diameter = this.Diameter,TextOne=TxtOne }) { Color=Color,PreColor=PreColor,ColorOfText=ColorOfText,PreColorOfText=PreColorOfText};
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
