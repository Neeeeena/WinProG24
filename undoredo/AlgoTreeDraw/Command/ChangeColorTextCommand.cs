using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AlgoTreeDraw.Command 
{
    class ChangeColorTextCommand : IUndoRedoCommand
    {
        private NodeViewModel _node { get; set; }
        private Brush _preColor { get; set; }
        private Brush _color { get; set; }

        public ChangeColorTextCommand(NodeViewModel node, Brush color, Brush preColor)
        {
            _node = node;
            _preColor = preColor;
            _color = color;
        }

        public override String ToString()
        {
            return "Color text";
        }

        public void Execute()
        {
            _node.ColorOfText = _color;
        }

        public void UnExecute()
        {
            _node.ColorOfText = _preColor;
        }
    }
}
