using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Walterlv.Events
{
    [TypeConverter(typeof (DeclarationChainMarkupConverter))]
    public class DeclarationChainCollection : Collection<DeclarationChain>
    {
        public DeclarationChainCollection()
        {
        }

        public DeclarationChainCollection(IList<DeclarationChain> list) : base(list)
        {
        }
    }
}
