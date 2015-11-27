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
        private NodeViewModel fromNode;
        private NodeViewModel toNode;

        public AddLineCommand(ObservableCollection<LineViewModel> _lines, LineViewModel _line, NodeViewModel _fromNode, NodeViewModel _toNode)
        {
            lines = _lines;
            line = _line;
            fromNode = _fromNode;
            toNode = _toNode;
        }

        public override String ToString()
        {
            return "Add line";
        }

        public void Execute()
        {
            lines.Add(line);
            fromNode.addNeighbour(toNode);
        }

        public void UnExecute()
        {
            lines.Remove(line);
            fromNode.removeNeighbour(toNode);
        }
    }
}
