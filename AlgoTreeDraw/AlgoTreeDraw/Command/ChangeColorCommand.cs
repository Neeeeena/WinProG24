using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AlgoTreeDraw.Command
{
    class ChangeColorCommand : IUndoRedoCommand
    {

        private NodeViewModel _node { get; set; }
        private Brush _preColor {get; set;}
        private Brush _color { get; set; }

        public ChangeColorCommand(NodeViewModel node, Brush color, Brush preColor)
        {
            _node = node;
            _preColor = preColor;
            _color = color;
        }

        public override String ToString()
        {
            return "Color node";
        }

        public void Execute()
        {
            _node.Color = _color;
            _node.PreColor = _color;
        }

        public void UnExecute()
        {
            _node.Color = _preColor;
            _node.PreColor = _preColor;
        }
    }
}
