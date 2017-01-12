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
                    var singleWordNodes = CreateNodes(nodeString);
                    int count = list.Count;
                    list.AddRange(singleWordNodes);
                    if (count == list.Count)
                    {
                        throw new ArgumentException($"Cannot recognize \"{nodeString}\" as {nameof(DeclarationChainNode)}.");
                    }
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
            var infos = parts.Skip(1).Select(x => (DE) Enum.Parse(typeof(DE), x.Trim()));
            return DeclarationChainNode.CreateNodes(key, infos);
        }
    }
}
