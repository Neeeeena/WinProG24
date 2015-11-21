using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AlgoTreeDraw.Command
{
    public class UndoRedo :ViewModelBase
    {

        public static UndoRedo Instance { get; } = new UndoRedo();
        public bool EnableUndo { get; set; }
        public bool EnableRedo { get; set; }
        private Stack<IUndoRedoCommand> _undoCommands = new Stack<IUndoRedoCommand>();
        private Stack<IUndoRedoCommand> _redoCommands = new Stack<IUndoRedoCommand>();

        public void InsertInUndoRedo(IUndoRedoCommand command)
        {
            _undoCommands.Push(command);
            _redoCommands.Clear();
            command.Execute();
            EnableUndo = CanUndo(1);
            RaisePropertyChanged(nameof(EnableUndo));
            RaisePropertyChanged(nameof(EnableRedo));
        }

        public bool CanRedo(int levels)
        {
            return levels <= _redoCommands.Count;
        }
        public bool CanUndo(int levels)
        {
            return levels <= _undoCommands.Count;
        }
        public void Redo(int levels)
        {
            if (!CanRedo(levels)) throw new InvalidOperationException();
            for(int i= 0;i<= levels; i++)
            {
                IUndoRedoCommand command = _redoCommands.Pop();
                command.Execute();
                _undoCommands.Push(command);
                EnableUndo = CanUndo(1);
                EnableRedo = CanRedo(1);
                RaisePropertyChanged(nameof(EnableUndo));
                RaisePropertyChanged(nameof(EnableRedo));

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
                EnableUndo = CanUndo(1);
                EnableRedo = CanRedo(1);
                RaisePropertyChanged(nameof(EnableUndo));
                RaisePropertyChanged(nameof(EnableRedo));

            }

        }


    }
}
