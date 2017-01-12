using System;
using System.Collections.Generic;
using System.Linq;

namespace Walterlv.Events
{
    public abstract class DeclarationEventBase
    {
        protected static void Register(string name, Type ownerType, CreateNodeCallback createNode)
        {
            DeclarationChain.Register(name, ownerType, createNode);
        }
    }

    public class HoldingEvent : DeclarationEventBase
    {
        static HoldingEvent()
        {
            Register("Holding", typeof(HoldingEvent), CreateNode);
        }

        private static IEnumerable<DeclarationChainNode> CreateNode(DE[] infos)
        {
            if (infos.Any())
            {
                throw new ArgumentException("HoldingEvent does not need any extra info.");
            }
            yield return new DownChainNode();
            yield return new MoveChainNode();
        }
    }

    public class TappedEvent : DeclarationEventBase
    {
        static TappedEvent()
        {
            Register("Tapped", typeof(TappedEvent), CreateNode);
        }

        private static IEnumerable<DeclarationChainNode> CreateNode(DE[] infos)
        {
            return Enumerable.Empty<DeclarationChainNode>();
        }
    }
}
