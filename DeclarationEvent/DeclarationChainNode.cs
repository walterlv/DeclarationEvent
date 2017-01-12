using System.Collections.Generic;

namespace Walterlv.Events
{
    /// <summary>
    /// Declaration event Chain Node.
    /// </summary>
    public abstract class DeclarationChainNode
    {
        protected DeclarationChainNode(IList<DE> infos)
        {
        }
    }

    public delegate IEnumerable<DeclarationChainNode> CreateNodeCallback(DE[] infos);

    public sealed class DownChainNode : DeclarationChainNode
    {
        public DownChainNode(IList<DE> infos) : base(infos)
        {
        }

        public DownChainNode(params DE[] infos) : base(infos)
        {
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
    }

    public sealed class UpChainNode : DeclarationChainNode
    {
        public UpChainNode(IList<DE> infos) : base(infos)
        {
        }

        public UpChainNode(params DE[] infos) : base(infos)
        {
        }
    }
}
