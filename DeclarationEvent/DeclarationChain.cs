using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Walterlv.Events
{
    /// <summary>
    /// Declaration Event Chain.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
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

        private void AddInner(DeclarationChainNode node)
        {
            _chains.Add(node);
        }

        public void Add(DeclarationChainNode node)
        {
            AddInner(node);
        }

        public DeclarationChain Down(params DE[] infos)
        {
            AddInner(new DownChainNode(infos));
            return this;
        }

        public DeclarationChain Up(params DE[] infos)
        {
            AddInner(new UpChainNode(infos));
            return this;
        }

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
