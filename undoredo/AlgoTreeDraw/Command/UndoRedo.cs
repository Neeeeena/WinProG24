using AlgoTreeDraw.ViewModel;
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
        //Start på mega mange variabler til undo redo menuen
        public string undoOne
        {
            get { return getUndoCommandFromIndex(0); }
        }
        public string undoTwo
        {
            get { return getUndoCommandFromIndex(1); }
        }
        public string undoThree
        {
            get { return getUndoCommandFromIndex(2); }
        }
        public string undoFour
        {
            get { return getUndoCommandFromIndex(3); }
        }
        public string undoFive
        {
            get { return getUndoCommandFromIndex(4); }
        }
        public string redoOne
        {
            get { return getRedoCommandFromIndex(0); }
        }
        public string redoTwo
        {
            get { return getRedoCommandFromIndex(1); }
        }
        public string redoThree
        {
            get { return getRedoCommandFromIndex(2); }
        }
        public string redoFour
        {
            get { return getRedoCommandFromIndex(3); }
        }
        public string redoFive
        {
            get { return getRedoCommandFromIndex(4); }
        }

        public IUndoRedoCommand[] undoList { get { return _undoCommands.ToArray(); } }
        public IUndoRedoCommand[] redoList { get { return _redoCommands.ToArray(); } }




        public string getUndoCommandFromIndex(int index)
        {
            if (_undoCommands.Count-1 >= index)
            {
                IUndoRedoCommand[] temp = _undoCommands.ToArray();
                return temp[index].ToString();
            }
            return "";
        }

        public string getRedoCommandFromIndex(int index)
        { 
            if(_redoCommands.Count-1 >= index)
            {
                IUndoRedoCommand[] temp = _redoCommands.ToArray();
                return temp[index].ToString();
            }
            return "";
            
        }

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
                EnableUndo = CanUndo(levels);
                EnableRedo = CanRedo(levels);
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
                EnableUndo = CanUndo(levels);
                EnableRedo = CanRedo(levels);
                RaisePropertyChanged(nameof(EnableUndo));
                RaisePropertyChanged(nameof(EnableRedo));

            }

        }


    }
}
