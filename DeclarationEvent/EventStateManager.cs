using System.Collections.Generic;
using System.Linq;

namespace Walterlv.Events
{
    internal sealed class EventStateManager
    {
        private Dictionary<DeclarationChain, int> _chainStepDictionary;

        internal EventStateManager(IEnumerable<DeclarationChain> nodes)
        {
            _chainStepDictionary = nodes.ToDictionary(x => x, x => 0);
        }

        internal void Step<T>(T node) where T : DeclarationChainNode
        {
            
        }
    }
}
