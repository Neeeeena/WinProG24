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
    /// Interaction logic for BSTUserControl.xaml
    /// </summary>
    public partial class BSTUserControl : UserControl
    {
        public BSTUserControl()
        {
            InitializeComponent();
        }

        /*public BSTUserControl(Canvas c)
        {
            InitializeComponent();
            this.Height = c.Height;
            this.Width = c.Width;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject();
                data.SetData("Double", Height);
                data.SetData("Object", this);

                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            e.Handled = true;
        }*/
       
    }
}
