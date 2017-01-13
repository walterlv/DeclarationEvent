using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Walterlv.Events
{
    public abstract class DeclarationChainNode
    {
        protected ReadOnlyCollection<DE> Infos { get; private set; }

        internal DeclarationChainNode(IList<DE> infos)
        {
            Infos = new ReadOnlyCollection<DE>(infos);
        }

        internal bool CanPassStep(DeclarationChainNode node)
        {
            return PassStep(node);
        }

        protected abstract bool PassStep(DeclarationChainNode node);
    }

    public abstract class DeclarationChainNode<T> : DeclarationChainNode where T : DeclarationChainNode
    {
        internal DeclarationChainNode(IList<DE> infos) : base(infos)
        {
        }

        protected sealed override bool PassStep(DeclarationChainNode node)
        {
            return PassStepCore((T) node);
        }

        protected abstract bool PassStepCore(T node);
    }

    public sealed class DownChainNode : DeclarationChainNode
    {
        public DownChainNode(IList<DE> infos) : base(infos)
        {
        }

        public DownChainNode(params DE[] infos) : base(infos)
        {
        }

        protected override bool PassStep(DeclarationChainNode node)
        {
            return true;
        }
    }

    public sealed class MoveChainNode : DeclarationChainNode
    {
        public MoveChainNode(IList<DE> infos) : base(infos)
        {
        }

        public MoveChainNode(params DE[] infos) : base(infos)
        {
        }

        protected override bool PassStep(DeclarationChainNode node)
        {
            return true;
        }
    }

    public sealed class UpChainNode : DeclarationChainNode
    {
        public UpChainNode(IList<DE> infos) : base(infos)
        {
        }

        public UpChainNode(params DE[] infos) : base(infos)
        {
        }

        protected override bool PassStep(DeclarationChainNode node)
        {
            return true;
        }
    }

    public sealed class DelayChainNode : DeclarationChainNode<DelayChainNode>
    {
        public DelayChainNode(IList<DE> infos) : base(infos)
        {
        }

        public DelayChainNode(params DE[] infos) : base(infos)
        {
        }

        protected override bool PassStepCore(DelayChainNode node)
        {
            return true;
        }
    }
}
