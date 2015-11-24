using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Command
{
    class AddLineCommand : IUndoRedoCommand
    {
        private ObservableCollection<LineViewModel> lines;
        private LineViewModel line;

        public AddLineCommand(ObservableCollection<LineViewModel> _lines, LineViewModel _line)
        {
            lines = _lines;
            line = _line;
        }

        public void Execute()
        {
            lines.Add(line);
        }

        public void UnExecute()
        {
           lines.Remove(line);
        }
    }
}
