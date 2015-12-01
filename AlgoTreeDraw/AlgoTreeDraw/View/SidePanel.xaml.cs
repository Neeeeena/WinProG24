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
    /// Interaction logic for MenuBar.xaml
    /// </summary>
    public partial class SidePanel : UserControl
    {
        public SidePanel()
        {
            InitializeComponent();
            Scroll.ScrollChanged += Scroll_ScrollChanged1;
        }
        private void Scroll_ScrollChanged1(object sender, ScrollChangedEventArgs e)
        {
            var spvm = (SidePanelViewModel)DataContext;
            spvm.VOffSP = Scroll.VerticalOffset;
        }

    }
}
