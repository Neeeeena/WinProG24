﻿using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AlgoTreeDraw.Model;

namespace AlgoTreeDraw.ViewModel
{
    public class T234ViewModel : NodeViewModel
    {
       
        public string TxtTwo { get { return ((T234)Node).TextTwo; } set { ((T234)Node).PreTextTwo = ((T234)Node).TextTwo; ((T234)Node).TextTwo = value; RaisePropertyChanged(); } }
        public string TxtThree { get { return ((T234)Node).TextThree; } set { ((T234)Node).PreTextThree = ((T234)Node).TextThree; ((T234)Node).TextThree = value; RaisePropertyChanged(); } }

        public string PreTxtTwo { get { return ((T234)Node).PreTextTwo; } set { ((T234)Node).PreTextTwo = value; RaisePropertyChanged(); } }
        public string PreTxtThree { get { return ((T234)Node).PreTextThree; } set { ((T234)Node).PreTextThree = value; RaisePropertyChanged(); } }

        public bool _IsTwoNode { get { return ((T234)Node).IsTwoNode; } set{ ((T234)Node).IsTwoNode = value;RaisePropertyChanged(); ShowCorrectNode(); } }
        public bool _IsThreeNode { get { return ((T234)Node).IsThreeNode; } set { ((T234)Node).IsThreeNode = value; RaisePropertyChanged(); ShowCorrectNode(); } }
        public bool _IsFourNode { get { return ((T234)Node).IsFourNode; } set { ((T234)Node).IsFourNode = value; RaisePropertyChanged(); ShowCorrectNode(); } }


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

        public new double CanvasCenterX { get {
                    if(((T234)Node).IsTwoNode) {
                        return X + Diameter / 2;
                    } else if(((T234)Node).IsThreeNode)
                    {
                        return X + Diameter;
                    } else
                    {
                        return X + (Diameter * 3) / 2;
                    }
                } set { X = value - Diameter / 2; RaisePropertyChanged(() => X); } }
        public new double CanvasCenterY { get {
                return Y + Diameter / 2;
                
            } set { Y = value - Diameter / 2; RaisePropertyChanged(() => Y); } }

        public void ShowCorrectNode()
        {
            if (_IsTwoNode)
            {
                ShowOneT234Node();
            }
            else if (_IsThreeNode)
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
            _IsTwoNode = true;
            _IsThreeNode = false;
            _IsFourNode = false;
            ShowCorrectNode();
        }

        public void ChangeTo3Node()
        {
            _IsTwoNode = false;
            _IsThreeNode = true;
            _IsFourNode = false;
            ShowCorrectNode();
        }
        public void ChangeTo4Node()
        {
            _IsTwoNode = false;
            _IsThreeNode = false;
            _IsFourNode = true;
            ShowCorrectNode();
        }


        public void ShowOneT234Node()
        {
            this.ShowNode1 = Visibility.Visible;
            this.ShowNode2 = Visibility.Hidden;
            this.ShowNode3 = Visibility.Hidden;
            RaisePropertyChanged(() => CanvasCenterX);
        }

        public void ShowTwoT234Node()
        {
            this.ShowNode1 = Visibility.Visible;
            this.ShowNode2 = Visibility.Visible;
            this.ShowNode3 = Visibility.Hidden;
            RaisePropertyChanged(() => CanvasCenterX);
        }

        public void ShowThreeT234Node()
        {
            this.ShowNode1 = Visibility.Visible;
            this.ShowNode2 = Visibility.Visible;
            this.ShowNode3 = Visibility.Visible;
            RaisePropertyChanged(() => CanvasCenterX);
        }

        public override NodeViewModel newNodeViewModel()
        {
            if (_IsThreeNode)
            {
                return new T234ViewModel(new T234() { X = this.X, Y = this.Y, diameter = this.Diameter, IsThreeNode = true,TextOne=TxtOne,TextTwo=TxtTwo}) { Color = Color, PreColor = PreColor, ColorOfText = ColorOfText, PreColorOfText = PreColorOfText };
            }
            else if(_IsTwoNode)
            {
                return new T234ViewModel(new T234() { X = this.X, Y = this.Y, diameter = this.Diameter, IsTwoNode = true,TextOne=TxtOne }) { Color = Color, PreColor = PreColor, ColorOfText = ColorOfText, PreColorOfText = PreColorOfText };
            }
            else
            {
                return new T234ViewModel(new T234() { X = this.X, Y = this.Y, diameter = this.Diameter, IsFourNode = true, TextOne = TxtOne, TextTwo = TxtTwo, TextThree=TxtThree }) { Color = Color, PreColor = PreColor, ColorOfText = ColorOfText, PreColorOfText = PreColorOfText };
            }
        }
        public void Merge(T234ViewModel other, T234ViewModel optional = null)
        {
            if (other != null)
            {
                if (_IsTwoNode && other._IsTwoNode)
                {
                    if(int.Parse(TxtOne) > int.Parse(other.TxtOne))
                    {
                        TxtTwo = TxtOne;
                        TxtOne = other.TxtOne;
                    }
                    else
                    {
                        TxtTwo = other.TxtOne;
                    }
                    
                    _IsTwoNode = false;
                    _IsThreeNode = true;
                }
                else if (_IsTwoNode && other._IsThreeNode)
            {
                    if(int.Parse(TxtOne) > int.Parse(other.TxtOne))
                    {
                        if(int.Parse(TxtOne) > int.Parse(other.TxtTwo))
                        {
                            TxtThree = TxtOne;
                            TxtOne = other.TxtOne;
                            TxtTwo = other.TxtTwo;
            }
                        else
            {
                            TxtTwo = TxtOne;
                            TxtOne = other.TxtOne;
                            TxtThree = other.TxtTwo;
                        }
            }
            else
            {
                        TxtTwo = other.TxtOne;
                        TxtThree = other.TxtTwo;
                    }

                    _IsTwoNode = false;
                    _IsFourNode = true;
                }
                if (optional != null && optional._IsTwoNode && _IsThreeNode)
                {
                    TxtThree = optional.TxtOne;
                    _IsThreeNode = false;
                    _IsFourNode = true;
                }
            }
        }

        public List<T234ViewModel> Split()
        {
            List<T234ViewModel> temp = new List<T234ViewModel>();
            if (_IsThreeNode)
            {
                temp.Add(new T234ViewModel(new T234() { TextOne = TxtTwo, IsTwoNode = true }));
                _IsThreeNode = false;
                _IsTwoNode = true;
            }
            else if (_IsFourNode)
            {
                temp.Add(new T234ViewModel (new T234() { TextOne = this.TxtTwo, IsTwoNode = true }));
                temp.Add(new T234ViewModel (new T234() { TextOne = this.TxtThree, IsTwoNode = true}));
                _IsFourNode = false;
                _IsTwoNode = true;

            }
            return temp;
        }

        public int Length()
        {
            if (_IsTwoNode)
            {
                return 30;
            }else if (_IsThreeNode)
            {
                return 60;
            }
            else
            {
                return 90;
            }
        }
    }
}
