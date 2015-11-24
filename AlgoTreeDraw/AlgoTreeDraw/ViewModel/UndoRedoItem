using AlgoTreeDraw.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlgoTreeDraw.ViewModel
{
    public class UndoRedoItem
    {
        public string Text { get; set; }
        public ICommand Command { get; set; }
        public UndoRedoItem(string item, ICommand command)
        {
            Text = item;
            Command = command;
        }
    }
}
