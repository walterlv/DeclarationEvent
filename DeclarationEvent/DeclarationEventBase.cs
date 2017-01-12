using System.Collections.Generic;
using System.Linq;

namespace Walterlv.Events
{
    public abstract class DeclarationEventBase
    {
        internal string GetName()
        {
            return Name;
        }

        internal DeclarationChain GetChain()
        {
            return new DeclarationChain(DefineChain());
        }

        protected abstract string Name { get; }

        protected abstract IEnumerable<DeclarationChainNode> DefineChain();
    }

    public class HoldingEvent : DeclarationEventBase
    {
        protected override string Name => "Holding";

        protected sealed override IEnumerable<DeclarationChainNode> DefineChain()
        {
            yield return new DownChainNode();
            yield return new MoveChainNode();
        }
    }

    public class TappedEvent : DeclarationEventBase
    {
        protected override string Name => "Tapped";

        protected override IEnumerable<DeclarationChainNode> DefineChain()
        {
            return Enumerable.Empty<DeclarationChainNode>();
        }
    }
}
