using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walterlv.Events
{
    internal sealed class EventStateManager
    {
        private Dictionary<DeclarationChain, int> _chainStepDictionary;

        internal EventStateManager(IEnumerable<DeclarationChain> nodes)
        {
            _chainStepDictionary = nodes.ToDictionary(x => x, x => 0);
        }

        internal void Step()
        {
            
        }
    }
}
