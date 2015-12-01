using AlgoTreeDraw.ViewModel;
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
    /// Interaction logic for T234UserControl.xaml
    /// </summary>
    public partial class T234UserControl : UserControl
    {
        public T234UserControl()
        {
            InitializeComponent();
        }
        private enum TextBoxx { txt1,txt2,txt3};
        private void updateKeys(TextBoxx txt)
        {
            var mvm = (MainViewModelBase)DataContext;
            T234ViewModel node = (T234ViewModel)mvm.editNode;
            if (node != null)
            {
                switch (txt)
                {
                    case TextBoxx.txt1:
                        node.TxtOne = textbox1.Text;
                        break;
                    case TextBoxx.txt2:
                        node.TxtTwo = textbox2.Text;
                        break;
                    case TextBoxx.txt3:
                        node.TxtThree = textbox3.Text;
                        break;
                }
                
            }
        }

        private void textbox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                updateKeys(TextBoxx.txt1);
            }
        }

        private void textbox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                updateKeys(TextBoxx.txt2);
            }
        }

        private void textbox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                updateKeys(TextBoxx.txt3);
            }
        }

        private void textbox_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if(txt.Name.Equals("textbox1"))
            {
                updateKeys(TextBoxx.txt1);
            }
            else if(txt.Name.Equals("textbox2"))
            {
                updateKeys(TextBoxx.txt2);
            } else
            {
                updateKeys(TextBoxx.txt3);
            }
        }
    }
}
