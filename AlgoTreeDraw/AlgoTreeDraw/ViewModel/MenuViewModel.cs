using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using AlgoTreeDraw.Model;
using AlgoTreeDraw.Serialization;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AlgoTreeDraw.ViewModel
{
    public class MenuViewModel : MainViewModelBase
    {

        public ICommand NewDiagramCommand { get; }
        public ICommand OpenDiagramCommand { get; }
        public ICommand SaveDiagramCommand { get; }
        public DialogBox dialogVM { get; set; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        public MenuViewModel()
        {
            NewDiagramCommand = new RelayCommand(NewDiagram);
            OpenDiagramCommand = new RelayCommand(OpenDiagram);
            SaveDiagramCommand = new RelayCommand(SaveDiagram);
            dialogVM = new DialogBox();
            UndoCommand = new RelayCommand<int>(undoClicked);
            RedoCommand = new RelayCommand<int>(redoClicked);
        }

        private Boolean _isUndoOpen;
        public Boolean isUndoOpen { get { return _isUndoOpen; }
            set { _isUndoOpen = value; RaisePropertyChanged(); RaisePropertyChanged(() => UndoCommands); Debug.Write(_isUndoOpen); } }

        private Boolean _isRedoOpen;
        public Boolean isRedoOpen { get { return _isRedoOpen; } set { _isRedoOpen = value; RaisePropertyChanged(() => RedoCommands); RaisePropertyChanged(); } }

        public void undoClicked(int level)
        {
            undoRedo.Undo(level);
            isUndoOpen = false;
            RaisePropertyChanged(() => RedoCommands);
            RaisePropertyChanged(() => UndoCommands);
            RaisePropertyChanged(() => isUndoOpen);
        }

        public void redoClicked(int level)
        {
            undoRedo.Redo(level);
            isRedoOpen = false;
            RaisePropertyChanged(() => RedoCommands);
            RaisePropertyChanged(() => UndoCommands);
            RaisePropertyChanged(() => isRedoOpen);
        }

        public ObservableCollection<UndoRedoItem> _RedoCommands { get; set; } = new ObservableCollection<UndoRedoItem>();
        public ObservableCollection<UndoRedoItem> _UndoCommands { get; set; } = new ObservableCollection<UndoRedoItem>();

        public ObservableCollection<UndoRedoItem> RedoCommands
        {

            get
            {
                _RedoCommands.Clear();
                if (undoRedo.redoList.Length != 0)
                {
                    int level = 0;
                    foreach (var c in undoRedo.redoList)
                    {
                        _RedoCommands.Add(new UndoRedoItem(c.ToString(), level, c));
                        level++;
                    }
                }

                return _RedoCommands;
            }

        }

        public ObservableCollection<UndoRedoItem> UndoCommands
        {

            get
            {
                _UndoCommands.Clear();
                if (undoRedo.undoList.Length != 0)
                {
                    int level = 0;
                    foreach (var c in undoRedo.undoList)
                    {
                        _UndoCommands.Add(new UndoRedoItem(c.ToString(), level, c));
                        level++;
                    }
                }
                return _UndoCommands;
            }


            /* get { return _RedoCommands; } set { _RedoCommands = value; Debug.Write("Heeeeey"); } */
        }

        //Kun for test
        public int one = 1;
        public int two = 2;
        public int three = 3;
        public int four = 4;
        public int five = 5;
        private void NewDiagram()
        {
            if (dialogVM.ShowNew())
            {
                Nodes.Clear();
                Lines.Clear();
            }
        }

        private async void OpenDiagram()
        {
            string path = dialogVM.ShowOpen();
            if (path != null)
            {
                Diagram diagram = await XMLserializer.Instance.AsyncDeserializeFromFile(path);

                Nodes.Clear();
                foreach(Node n in diagram.Nodes)
                {
                    if(n is BST)
                    {
                        Nodes.Add(new BSTViewModel(n));
                        
                    }else if(n is RBT)
                    {
                        Nodes.Add(new RBTViewModel(n));
                    }
                    else
                    {
                        Nodes.Add(new T234ViewModel((T234)n));
                    }
                }
                Lines.Clear();
                diagram.Lines.Select(l => new LineViewModel(l)).ToList().ForEach(l => Lines.Add(l));

                // Reconstruct object graph.
                foreach (LineViewModel line in Lines)
                {
                    line.From = Nodes.Single(n => line.Line.From.X == n.Node.X && line.Line.From.Y == n.Node.Y);
                    line.To = Nodes.Single(n => line.Line.To.X == n.Node.X && line.Line.To.Y == n.Node.Y);
                }
            }
        }
        //commandd to save diagram
        private void SaveDiagram()
        {
            string path = dialogVM.ShowSave();
            if (path != null)
            {
                Diagram diagram = new Diagram() { Nodes = Nodes.Select(n => n.Node).ToList(), Lines = Lines.Select(l => l.Line).ToList() };
                XMLserializer.Instance.AsyncSerializeToFile(diagram, path);
            }
        }


    }
}
