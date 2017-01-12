using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Walterlv.Events
{
    /// <summary>
    /// Declaration Event Chain.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
    public class DeclarationChain : ICollection<DeclarationChainNode>
    {
        private readonly List<DeclarationChainNode> _chains;

        public DeclarationChain()
        {
            _chains = new List<DeclarationChainNode>();
        }

        public DeclarationChain(IEnumerable<DeclarationChainNode> nodes)
        {
            _chains = nodes.ToList();
        }

        public void Add(DeclarationChainNode node)
        {
            _chains.Add(node);
        }

        #region Extensions

        private static readonly Dictionary<string, CreateNodeCallback>
            ConverterDictionary = new Dictionary<string, CreateNodeCallback>
            {
                {"Down", data => new DeclarationChainNode[] {new DownChainNode(data)}},
                {"Move", data => new DeclarationChainNode[] {new MoveChainNode(data)}},
                {"Up", data => new DeclarationChainNode[] {new UpChainNode(data)}},
            };

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

        #endregion

        #region Debugger

        private string DebuggerDisplay => "DeclarationChain" + string.Join("-",
                                              _chains.Select(x => x.GetType().Name.Replace("ChainNode", "")));

        #endregion

        #region Collection

        int ICollection<DeclarationChainNode>.Count => _chains.Count;

        bool ICollection<DeclarationChainNode>.IsReadOnly => false;

        bool ICollection<DeclarationChainNode>.Remove(DeclarationChainNode node)
        {
            return _chains.Remove(node);
        }

        void ICollection<DeclarationChainNode>.Clear()
        {
            _chains.Clear();
        }

        bool ICollection<DeclarationChainNode>.Contains(DeclarationChainNode node)
        {
            return _chains.Contains(node);
        }

        void ICollection<DeclarationChainNode>.CopyTo(DeclarationChainNode[] array, int arrayIndex)
        {
            _chains.CopyTo(array, arrayIndex);
        }

        IEnumerator<DeclarationChainNode> IEnumerable<DeclarationChainNode>.GetEnumerator()
        {
            return _chains.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _chains.GetEnumerator();
        }

        #endregion
    }
}
