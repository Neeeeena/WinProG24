using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Command
{
    // Custom interface for implementing Undo/Redo commands.
    public interface IUndoRedoCommand
    {

        // For doing and redoing the command.
        void Execute();
        // For undoing the command.
        void UnExecute();

    }
}
