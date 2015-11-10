using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Command
{
    public class UndoRedo
    {

        public static UndoRedo Instance { get; } = new UndoRedo();
        private Stack<IUndoRedoCommand> _undoCommands = new Stack<IUndoRedoCommand>();
        private Stack<IUndoRedoCommand> _redoCommands = new Stack<IUndoRedoCommand>();

        public void InsertInUndoRedo(IUndoRedoCommand command)
        {
            _undoCommands.Push(command);
            _redoCommands.Clear();
            command.Execute();
        }

        public bool CanRedo(int levels)
        {
            return levels <= _undoCommands.Count;
        }
        public bool CanUndo(int levels)
        {
            return levels <= _redoCommands.Count;
        }
        public void Redo(int levels)
        {
            if (!CanRedo(levels)) throw new InvalidOperationException();
            for(int i= 0;i<= levels; i++)
            {
                 IUndoRedoCommand command = _redoCommands.Pop();
                 command.Execute();
                 _undoCommands.Push(command);

            }
        }
        public void Undo(int levels)
        {
            if (!CanUndo(levels)) throw new InvalidOperationException();
            for (int i = 0; i <= levels; i++)
            {

                IUndoRedoCommand command = _undoCommands.Pop();
                command.UnExecute();
                _redoCommands.Push(command);

            }

        }


    }
}
