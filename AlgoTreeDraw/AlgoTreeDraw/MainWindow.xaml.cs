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

namespace AlgoTreeDraw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void RDChecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = null;
            rb = e.OriginalSource as RadioButton;
            if (String.Equals(rb.Content, "Mathias"))
            {
                MessageBox.Show("You are absolutely right!");
            }
            else
            {
                Random random = new Random();
                int n = random.Next(0, 4);
                String[] answers = { "No!", "Wrong!", "Not Likely","Er du dum?" };
                MessageBox.Show(answers[n]);
            }
        }
    }
}
