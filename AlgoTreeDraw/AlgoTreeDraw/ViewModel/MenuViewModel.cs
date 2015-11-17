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

namespace AlgoTreeDraw.ViewModel
{
    public class MenuViewModel : MainViewModelBase
    {

        public ICommand NewDiagramCommand { get; }
        public ICommand OpenDiagramCommand { get; }
        public ICommand SaveDiagramCommand { get; }
        public DialogBox dialogVM { get; set; }
        public MenuViewModel()
        {
            NewDiagramCommand = new RelayCommand(NewDiagram);
            OpenDiagramCommand = new RelayCommand(OpenDiagram);
            SaveDiagramCommand = new RelayCommand(SaveDiagram);

            dialogVM = new DialogBox();

        }
        //Kun for test
        public int undoRedoParam = 1;
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
                        Nodes.Add(new T234ViewModel(n, 1));
                    }
                }
                Lines.Clear();
                diagram.Lines.Select(l => new LineViewModel(l)).ToList().ForEach(l => Lines.Add(l));

                // Reconstruct object graph.
                foreach (LineViewModel line in Lines)
                {
                    line.From = Nodes.Single(n => n.Node == line.Line.From);
                    line.To = Nodes.Single(n => n.Node == line.Line.To);
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
