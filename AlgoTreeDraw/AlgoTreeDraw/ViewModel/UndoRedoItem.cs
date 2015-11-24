using AlgoTreeDraw.Command;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlgoTreeDraw.ViewModel
{
    public class UndoRedoItem : MenuViewModel
    {
        public string Text { get; set; }
        private int level { get; set; }
        public IUndoRedoCommand Command { get; set; }
        public ICommand redo { get; set; }
        public ICommand undo { get; set; }
        public UndoRedoItem(string item, int level, IUndoRedoCommand Command)
        {
            Text = item;
            this.level = level;

            this.Command = Command;
            redo = new RelayCommand(_redoClicked);
            undo = new RelayCommand(_undoClicked);
        }

        private void _redoClicked()
        {
            redoClicked(level);
        }

        private void _undoClicked()
        {
            undoClicked(level);
        }


    }
}
