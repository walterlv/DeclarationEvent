using System.Collections.Generic;

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
            yield return new DownChainNode(DE.Single);
            yield return new DelayChainNode(DE.Long);
        }
    }

    public class TappedEvent : DeclarationEventBase
    {
        protected override string Name => "Tapped";

        protected override IEnumerable<DeclarationChainNode> DefineChain()
        {
            yield return new DownChainNode(DE.Single);
            yield return new DelayChainNode(DE.Long);
            yield return new UpChainNode(DE.Single);
        }
    }
}
