using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw
{
    class UndoRedoEnabledMsg
    {
        public bool undo { get; set; }
        public bool redo { get; set; }
        public UndoRedoEnabledMsg(bool undo, bool redo)
        {
            this.undo = undo;
            this.redo = redo;
        }
    }
}
