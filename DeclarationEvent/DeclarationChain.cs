using System.Collections;
using System.Collections.Generic;

namespace Walterlv.Events
{
    /// <summary>
    /// Declaration Event Chain.
    /// </summary>
    public class DeclarationChain : ICollection<DeclarationChainNode>
    {
        private readonly List<DeclarationChainNode> _chains = new List<DeclarationChainNode>();
        
        private void AddInner(DeclarationChainNode node)
        {
            _chains.Add(node);
        }
        
        public void Add(DeclarationChainNode node)
        {
            AddInner(node);
        }
        
        public DeclarationChain Down(params DM[] metas)
        {
            AddInner(new DownChainNode());
            return this;
        }

        public DeclarationChain Up(params DM[] metas)
        {
            AddInner(new UpChainNode());
            return this;
        }

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
