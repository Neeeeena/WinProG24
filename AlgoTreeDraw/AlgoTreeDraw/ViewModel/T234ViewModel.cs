using AlgoTreeDraw.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AlgoTreeDraw.ViewModel
{
    public class T234ViewModel : NodeViewModel
    {
        public string TxtOne { get{ return ((T234)Node).TextOne; }set{ ((T234)Node).TextOne = value; RaisePropertyChanged(); } }
        public string TxtTwo { get { return ((T234)Node).TextTwo; } set { ((T234)Node).TextTwo = value; RaisePropertyChanged(); } }
        public string TxtThree { get { return ((T234)Node).TextThree; } set { ((T234)Node).TextThree = value; RaisePropertyChanged(); } }

        public T234ViewModel(T234 _node) : base(_node)
        {
            ShowOneT234 = new RelayCommand(ChangeTo2Node);
            ShowTwoT234 = new RelayCommand(ChangeTo3Node);
            ShowThreeT234 = new RelayCommand(ChangeTo4Node);
            ShowCorrectNode();
            _ColorOfText = Brushes.Black;


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

        public void ShowCorrectNode()
        {
            if (((T234)Node).IsTwoNode)
            {
                ShowOneT234Node();
            }
            else if (((T234)Node).IsThreeNode)
            {
                ShowTwoT234Node();
            }
            else
            {
                ShowThreeT234Node();
            }
        }

        public void ChangeTo2Node()
        {
            ((T234)Node).IsTwoNode = true;
            ((T234)Node).IsThreeNode = false;
            ((T234)Node).IsFourNode = false;
            ShowCorrectNode();
        }

        public void ChangeTo3Node()
        {
            ((T234)Node).IsTwoNode = false;
            ((T234)Node).IsThreeNode = true;
            ((T234)Node).IsFourNode = false;
            ShowCorrectNode();
        }
        public void ChangeTo4Node()
        {
            ((T234)Node).IsTwoNode = false;
            ((T234)Node).IsThreeNode = false;
            ((T234)Node).IsFourNode = true;
            ShowCorrectNode();
        }


        public void ShowOneT234Node()
        {
            this.ShowNode1 = Visibility.Visible;
            this.ShowNode2 = Visibility.Hidden;
            this.ShowNode3 = Visibility.Hidden;
        }

        public void ShowTwoT234Node()
        {

            this.ShowNode1 = Visibility.Visible;
            this.ShowNode2 = Visibility.Visible;
            this.ShowNode3 = Visibility.Hidden;
        }

        public void ShowThreeT234Node()
        {
            this.ShowNode1 = Visibility.Visible;
            this.ShowNode2 = Visibility.Visible;
            this.ShowNode3 = Visibility.Visible;
        }

        public override NodeViewModel newNodeViewModel()
        {
            if (((T234)Node).IsThreeNode)
            {
                return new T234ViewModel(new T234() { X = this.X, Y = this.Y, diameter = this.Diameter, IsThreeNode = true});
            }
            else if(((T234)Node).IsTwoNode)
            {
                return new T234ViewModel(new T234() { X = this.X, Y = this.Y, diameter = this.Diameter, IsTwoNode = true });
            }
            else
            {
                return new T234ViewModel(new T234() { X = this.X, Y = this.Y, diameter = this.Diameter, IsFourNode = true });
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
