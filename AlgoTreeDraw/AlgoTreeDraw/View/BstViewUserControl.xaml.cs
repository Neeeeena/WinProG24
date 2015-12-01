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
    /// Interaction logic for BstView.xaml
    /// </summary>
    public partial class BstViewUserControl : UserControl
    {
        public BstViewUserControl()
        {
            InitializeComponent();
            NodeTextBox.TextChanged += NodeTextBox_TextChanged;
        }

        private void NodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var mvm = (MainViewModelBase)DataContext;
            NodeViewModel node = mvm.editNode;
            if(node != null)
            {
                node.Key = NodeTextBox.Text;
            }
            
        }

        private void NodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                var mvm = (MainViewModelBase)DataContext;
                mvm._DoneEditing();
            }
        }
    }
}
