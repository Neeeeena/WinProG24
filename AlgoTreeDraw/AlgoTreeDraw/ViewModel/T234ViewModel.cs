using AlgoTreeDraw.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AlgoTreeDraw.ViewModel
{
    class T234ViewModel : NodeViewModel
    {
        public T234ViewModel(Node _node,int n) : base(_node)
        {
            ShowOneT234 = new RelayCommand<MouseButtonEventArgs>(ShowOneT234Node);
            ShowTwoT234 = new RelayCommand<MouseButtonEventArgs>(ShowTwoT234Node);
            ShowThreeT234 = new RelayCommand<MouseButtonEventArgs>(ShowThreeT234Node);
            switch (n) {
                case 1:
                    ShowOneT234Node(null);
                    break;
                case 2:
                    ShowTwoT234Node(null);
                    break;
                default:
                    ShowThreeT234Node(null);
                    break;
            }
        }

        public ICommand ShowOneT234 { get; }
        public ICommand ShowTwoT234 { get; }
        public ICommand ShowThreeT234 { get; }

        
        public Visibility _ShowNode1 = Visibility.Visible;
        public Visibility _ShowNode2 = Visibility.Hidden;
        public Visibility _ShowNode3 = Visibility.Hidden;

        public Visibility ShowNode1 { get { return _ShowNode1; } set { _ShowNode1 = value; RaisePropertyChanged(); } }
        public Visibility ShowNode2 { get { return _ShowNode2; } set { _ShowNode2 = value; RaisePropertyChanged(); } }
        public Visibility ShowNode3 { get { return _ShowNode3; } set { _ShowNode3 = value; RaisePropertyChanged(); } }

        public void ShowOneT234Node(MouseButtonEventArgs e)
        {
            this.ShowNode1 = Visibility.Visible;
            this.ShowNode2 = Visibility.Hidden;
            this.ShowNode3 = Visibility.Hidden;
        }

        public void ShowTwoT234Node(MouseButtonEventArgs e)
        {

            this.ShowNode1 = Visibility.Visible;
            this.ShowNode2 = Visibility.Visible;
            this.ShowNode3 = Visibility.Hidden;
        }

        public void ShowThreeT234Node(MouseButtonEventArgs e)
        {
            this.ShowNode1 = Visibility.Visible;
            this.ShowNode2 = Visibility.Visible;
            this.ShowNode3 = Visibility.Visible;
        }

        public override NodeViewModel newNodeViewModel()
        {
            if(this.ShowNode3 == Visibility.Visible)
            {
                return new T234ViewModel(new T234(),3);
            }
            else if(this.ShowNode2 == Visibility.Visible)
            {
                return new T234ViewModel(new T234(), 2);
            }
            else
            {
                return new T234ViewModel(new T234() {diameter =30},1);
            }
            
        }
        //public void MouseDoubleClickNode(MouseButtonEventArgs e)
        //{
        //    var node = TargetShape(e);
        //    if (!(node.isTextBoxVisible == Visibility.Visible))
        //    {

        //        node.isTextBoxVisible = Visibility.Visible;
        //    }
        //    else
        //    {
        //        node.isTextBoxVisible = Visibility.Hidden;
        //    }
        //    MessageBox.Show("lol");
        //}
    }
}
