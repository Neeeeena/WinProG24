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

        private List<NodeViewModel> selNodes = new List<NodeViewModel>();

        // The 'offsetX' field holds the offset (difference) between the original and final X coordinate.
        private double offsetX;
        // The 'offsetY' field holds the offset (difference) between the original and final Y coordinate.
        private double offsetY;



        public MoveNodeCommand(List<NodeViewModel> selectedNodes, double _offsetX, double _offsetY) 
        {
            foreach(NodeViewModel n in selectedNodes)
            {
                selNodes.Add(n);
            }
            offsetX = _offsetX;
            offsetY = _offsetY;
        }

        public void Execute()
        {
            foreach (var node in selNodes)
            {
                node.CanvasCenterX += offsetX;
                node.CanvasCenterY += offsetY;
            }
        }

        public void UnExecute()
        {
            foreach(var node in selNodes)
            {
                node.CanvasCenterX -= offsetX;
                node.CanvasCenterY -= offsetY;
            }
            
        }
    }
}
