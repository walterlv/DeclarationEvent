using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Walterlv.Events
{
    internal class DeclarationChainMarkupConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof (string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var text = value?.ToString();
            if (string.IsNullOrEmpty(text)) return null;

            var chains = new List<DeclarationChain>();

            foreach (var chainString in text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var chainNodeStrings = chainString.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
                var nodes = chainNodeStrings.Aggregate(new List<DeclarationChainNode>(), (list, nodeString) =>
                {
                    list.AddRange(CreateNodes(nodeString));
                    return list;
                });
                chains.Add(new DeclarationChain(nodes));
            }
            
            return new DeclarationChainCollection(chains);
        }

        private IEnumerable<DeclarationChainNode> CreateNodes(string nodeString)
        {
            var parts = nodeString.Split(new[] {'(', ')', '|'}, StringSplitOptions.RemoveEmptyEntries);
            var key = parts[0].Trim();
            var metadataList = parts.Skip(1).Select(x => DM.Parse(x.Trim()));
            return DeclarationEvent.CreateNodes(key, metadataList);
        }
    }
}
