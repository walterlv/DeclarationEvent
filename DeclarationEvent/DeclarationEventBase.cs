using System.Collections.Generic;
using static Walterlv.Events.DE;

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
            yield return new DownChainNode(Single);
            yield return new MoveChainNode(Near, Optional);
            yield return new DelayChainNode(Long);
        }
    }

    public class TappedEvent : DeclarationEventBase
    {
        protected override string Name => "Tapped";

        protected override IEnumerable<DeclarationChainNode> DefineChain()
        {
            yield return new DownChainNode(Single);
            yield return new MoveChainNode(Near, Optional);
            yield return new DelayChainNode(Short, Optional);
            yield return new UpChainNode(Single);
        }
    }

    public class DragEvent : DeclarationEventBase
    {
        protected override string Name => "Drag";

        protected override IEnumerable<DeclarationChainNode> DefineChain()
        {
            yield return new DownChainNode(Merge);
            yield return new MoveChainNode(Far, Confirmed);
            yield return new UpChainNode(Merge);
        }
    }
}
