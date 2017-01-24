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
    public class DeclarationChain : IList<DeclarationChainNode>
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

        public DeclarationChainNode this[int index]
        {
            get { return _chains[index]; }
            set { _chains[index] = value; }
        }

        public int Count => _chains.Count;

        public void Add(DeclarationChainNode node)
        {
            _chains.Add(node);
        }

        #region Debugger

        private string DebuggerDisplay => "DeclarationChain: " + string.Join("-",
                                              _chains.Select(x => x.GetType().Name.Replace("ChainNode", "")));

        #endregion

        #region Collection

        bool ICollection<DeclarationChainNode>.IsReadOnly => false;

        int IList<DeclarationChainNode>.IndexOf(DeclarationChainNode node)
        {
            return _chains.IndexOf(node);
        }

        void IList<DeclarationChainNode>.Insert(int index, DeclarationChainNode node)
        {
            _chains.Insert(index, node);
        }

        bool ICollection<DeclarationChainNode>.Remove(DeclarationChainNode node)
        {
            return _chains.Remove(node);
        }

        void IList<DeclarationChainNode>.RemoveAt(int index)
        {
            _chains.RemoveAt(index);
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
