using AlgoTreeDraw.Model;
using AlgoTreeDraw.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTreeDraw.Command
{
    public class TextChangeCommandT234 : IUndoRedoCommand
    {
        private T234ViewModel _node { get; set; }
        private string _preKey { get; set;  }
        private string _key { get; set; }
        private int _type { get; set; }

        public TextChangeCommandT234(T234ViewModel node, string key, string preKey, int type)
        {
            _node = node;
            _preKey = preKey;
            _key = key;
            _type = type;
        }

        public override String ToString()
        {
            return "Edit text";
        }

        public void Execute()
        {
            if(_type == 0)
            {
                _node.TxtOne = _key;
            } else if(_type == 1) {
                _node.TxtTwo = _key;
            } else {
                _node.TxtThree = _key;
            }
            
        }

        public void UnExecute()
        {
            if (_type == 0)
            {
                _node.TxtOne = _preKey;
            }
            else if (_type == 1)
            {
                _node.TxtTwo = _preKey;
            }
            else
            {
                _node.TxtThree = _preKey;
            }
        }
    }
}
