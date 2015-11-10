﻿using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
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

        public ChangeColorCommand(NodeViewModel node, Brush preColor, Brush color)
        {
            _node = node;
            _preColor = preColor;
            _color = color;
        }

        public void Execute()
        {
            _node.Color = _color;
        }

        public void UnExecute()
        {
            _node.Color = _preColor;
        }
    }
}
