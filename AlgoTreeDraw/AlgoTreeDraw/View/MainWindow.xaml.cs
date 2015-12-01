using AlgoTreeDraw.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlgoTreeDraw.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Send(MainGrid);
            Messenger.Default.Register<Cursor>(this, updateCursor);
            Scroll.ScrollChanged += Scroll_ScrollChanged1;
        }

        private void Scroll_ScrollChanged1(object sender, ScrollChangedEventArgs e)
        {
            var mvm = (MainViewModel)DataContext;
            mvm.VOff = Scroll.VerticalOffset;
        }

        private void updateCursor(Cursor Cursor)
        {
            this.Cursor = Cursor;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Scroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
           
        }
    }
}
