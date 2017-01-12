using System;
using System.Collections.Generic;
using System.Linq;

namespace Walterlv.Events
{
    /// <summary>
    /// Declaration event Chain Node.
    /// </summary>
    public abstract class DeclarationChainNode
    {
        private static readonly Dictionary<string, CreateNodeCallback>
            ConverterDictionary = new Dictionary<string, CreateNodeCallback>();

        private static readonly Dictionary<Type, string>
            NodeMetadataDictionary = new Dictionary<Type, string>();

        internal static IEnumerable<DeclarationChainNode> CreateNodes(string key, IEnumerable<DE> infos)
        {
            CreateNodeCallback func;
            if (ConverterDictionary.TryGetValue(key, out func))
            {
                return func(infos as DE[] ?? infos.ToArray());
            }
            return Enumerable.Empty<DeclarationChainNode>();
        }

        public static void Register(string name, Type ownerType, CreateNodeCallback createNode)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (createNode == null) throw new ArgumentNullException(nameof(createNode));
            if (ConverterDictionary.ContainsKey(name))
            {
                throw new ArgumentException($"Key \"{name}\" already exists", nameof(name));
            }

            ConverterDictionary.Add(name, createNode);
        }

        protected DeclarationChainNode(IList<DE> infos)
        {

        }
    }

    public delegate IEnumerable<DeclarationChainNode> CreateNodeCallback(DE[] infos);

    public sealed class DownChainNode : DeclarationChainNode
    {
        static DownChainNode()
        {
            Register("Down", typeof(DownChainNode), data => new DeclarationChainNode[] {new DownChainNode(data)});
        }

        public DownChainNode(IList<DE> infos) : base(infos)
        {
        }
    }

    public sealed class UpChainNode : DeclarationChainNode
    {
        static UpChainNode()
        {
            Register("Up", typeof(UpChainNode), data => new DeclarationChainNode[] {new UpChainNode(data)});
        }

        public UpChainNode(IList<DE> infos) : base(infos)
        {
        }
    }

    public sealed class MoveChainNode : DeclarationChainNode
    {
        static MoveChainNode()
        {
            Register("Move", typeof(MoveChainNode), data => new DeclarationChainNode[] {new MoveChainNode(data)});
        }

        public MoveChainNode(IList<DE> infos) : base(infos)
        {
        }
    }
}
