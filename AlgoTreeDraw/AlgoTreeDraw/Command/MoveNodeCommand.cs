using AlgoTreeDraw.Model;
using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Command
{
    public class MoveNodeCommand : IUndoRedoCommand
    {

        private List<NodeViewModel> nodes;

        // The 'offsetX' field holds the offset (difference) between the original and final X coordinate.
        private double offsetX;
        // The 'offsetY' field holds the offset (difference) between the original and final Y coordinate.
        private double offsetY;



        public MoveNodeCommand(List<NodeViewModel> _nodes, double _offsetX, double _offsetY) 
        {
            nodes = _nodes;
            offsetX = _offsetX;
            offsetY = _offsetY;
        }

        public override String ToString()
        {
            return "Move node";
        }

        public void Execute()
        {
            foreach (var node in nodes)
            {
                node.CanvasCenterX += offsetX;
                node.CanvasCenterY += offsetY;
            }
        }

        public void UnExecute()
        {
            foreach(var node in nodes)
            {
                node.CanvasCenterX -= offsetX;
                node.CanvasCenterY -= offsetY;
            }
            
        }
    }
}
