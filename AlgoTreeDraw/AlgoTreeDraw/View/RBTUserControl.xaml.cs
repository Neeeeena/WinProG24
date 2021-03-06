﻿using AlgoTreeDraw.ViewModel;
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
    /// Interaction logic for RBTUserControl.xaml
    /// </summary>
    public partial class RBTUserControl : UserControl
    {
        public RBTUserControl()
        {
            InitializeComponent();
        }

        private void NodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateKeys();
            }
        }

        private void UpdateKeys()
        {
            var mvm = (MainViewModelBase)DataContext;
            NodeViewModel node = mvm.editNode;
            if (node != null)
            {
                node.TxtOne = NodeTextBox.Text;
                mvm._DoneEditing();
            }

        }

        private void NodeTextBox_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            UpdateKeys();
        }
    }
}
