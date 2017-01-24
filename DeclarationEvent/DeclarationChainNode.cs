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

        internal StateStep CanPassStep(DeclarationChainNode node)
        {
            if (node.Infos.Contains(DE.Optional))
            {
                return StateStep.Ignore;
            }
            return PassStep(node);
        }

        /// <summary>
        /// 派生类重写此方法时，决定参数 <paramref name="node"/> 的实例是否符合自身的要求。
        /// 具体是指：参数表示实际发生的事件节点，自身表示预设的事件节点，此判断为决定实际发生的事件是否达到预设要求。
        /// <para></para>
        /// 如果达到要求，则返回 <see cref="StateStep.Pass"/>；
        /// 如果无此要求，则返回 <see cref="StateStep.Ignore"/>；
        /// 如果要求不满足，则返回 <see cref="StateStep.Abort"/>。
        /// </summary>
        /// <param name="node">实际发生的事件节点。</param>
        /// <returns></returns>
        protected abstract StateStep PassStep(DeclarationChainNode node);
    }

    public sealed class DownChainNode : DeclarationChainNode
    {
        public DownChainNode(IList<DE> infos) : base(infos)
        {
        }

        public DownChainNode(params DE[] infos) : base(infos)
        {
        }

        protected override StateStep PassStep(DeclarationChainNode node)
        {
            return StateStep.Pass;
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

        protected override StateStep PassStep(DeclarationChainNode node)
        {

            return StateStep.Pass;
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

        protected override StateStep PassStep(DeclarationChainNode node)
        {
            return StateStep.Pass;
        }
    }

    public sealed class DelayChainNode : DeclarationChainNode
    {
        public DelayChainNode(IList<DE> infos) : base(infos)
        {
        }

        public DelayChainNode(params DE[] infos) : base(infos)
        {
        }

        protected override StateStep PassStep(DeclarationChainNode node)
        {
            return StateStep.Pass;
        }
    }
}
