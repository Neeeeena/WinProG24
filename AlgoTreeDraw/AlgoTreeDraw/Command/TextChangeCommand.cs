using AlgoTreeDraw.Model;
using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Command
{
    public class TextChangeCommand : IUndoRedoCommand
    {
        private NodeViewModel _node { get; set; }
        private string _preKey { get; set;  }
        private string _key { get; set; }

        public TextChangeCommand(NodeViewModel node, string key, string preKey)
        {
            _node = node;
            _preKey = preKey;
            _key = key;
        }

        public override String ToString()
        {
            return "Edit text";
        }

        public void Execute()
        {
            _node.Key = _key;
        }

        public void UnExecute()
        {
            _node.Key = _preKey;
        }
    }
}
