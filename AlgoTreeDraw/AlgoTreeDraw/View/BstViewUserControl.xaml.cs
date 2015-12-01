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
        }

        private void NodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                var mvm = (MainViewModelBase)DataContext;
                NodeViewModel node = mvm.editNode;
                node.Key = NodeTextBox.Text;
                mvm._DoneEditing();
            }
        }
    }
}
