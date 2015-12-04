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
using Xceed.Wpf.Toolkit;

namespace AlgoTreeDraw.View
{
    /// <summary>
    /// Interaction logic for MenuBar.xaml
    /// </summary>
    public partial class MenuBar : UserControl
    {
        
        public MenuBar()
        {
            InitializeComponent();
            this.DataContext = new MenuViewModel();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.UndoBtn.IsEnabled = false;
            this.UndoBtn2.IsEnabled = false;
            UndoMenu.IsEnabled = false;
            RedoMenu.IsEnabled = false;
            Messenger.Default.Register<UndoRedoEnabledMsg>(this, updateUndoRedoEnabled);
        }

        private void updateUndoRedoEnabled(UndoRedoEnabledMsg msg)
        {
            this.UndoBtn.IsEnabled = msg.undo;
            this.UndoBtn2.IsEnabled = msg.redo;
            UndoMenu.IsEnabled = msg.undo;
            RedoMenu.IsEnabled = msg.redo;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UndoBtn.IsOpen = false;
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            UndoBtn2.IsOpen = false;
        }


    }
}
