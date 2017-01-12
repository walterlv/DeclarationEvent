using System.Collections.Generic;

namespace Walterlv.Events
{
    /// <summary>
    /// Declaration event Chain Node.
    /// </summary>
    public abstract class DeclarationChainNode
    {
        protected DeclarationChainNode(IList<DM> metadataList)
        {

        }
    }

    public sealed class DownChainNode : DeclarationChainNode
    {
        public DownChainNode(IList<DM> metadataList) : base(metadataList)
        {
        }
    }

    public sealed class UpChainNode : DeclarationChainNode
    {
        public UpChainNode(IList<DM> metadataList) : base(metadataList)
        {
        }
    }

    public sealed class DelayChainNode : DeclarationChainNode
    {
        public DelayChainNode(IList<DM> metadataList) : base(metadataList)
        {
        }
    }
}
