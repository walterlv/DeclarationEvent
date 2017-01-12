using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Walterlv.Events
{
    public sealed class DeclarationEvent
    {
        public static readonly DependencyProperty IsHostProperty =
            DependencyProperty.RegisterAttached(
                "IsHost", typeof (bool), typeof (DeclarationEvent),
                new PropertyMetadata(false, OnIsHostChanged));

        public static bool GetIsHost(DependencyObject element)
        {
            return (bool) element.GetValue(IsHostProperty);
        }

        public static void SetIsHost(DependencyObject element, bool value)
        {
            element.SetValue(IsHostProperty, value);
        }

        private static void OnIsHostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement element = (UIElement) d;
            var controller = (DeclarationEventController) element.GetValue(ControllerProperty);
            if (true.Equals(e.OldValue))
            {
                controller?.Detach();
            }
            if (true.Equals(e.NewValue))
            {
                controller?.Attach(element);
            }
        }

        public static readonly DependencyProperty EnabledChainsProperty =
            DependencyProperty.RegisterAttached(
                "EnabledChains", typeof (ICollection<DeclarationChain>), typeof (DeclarationEvent),
                new PropertyMetadata(default(ICollection<DeclarationChain>)));

        public static ICollection<DeclarationChain> GetEnabledChains(DependencyObject element)
        {
            var chains = (ICollection<DeclarationChain>) element.GetValue(EnabledChainsProperty) ??
                         new List<DeclarationChain>();
            return chains;
        }

        public static void SetEnabledChains(DependencyObject element, ICollection<DeclarationChain> value)
        {
            if (value == null)
            {
                element.ClearValue(EnabledChainsProperty);
            }
            else
            {
                element.SetValue(EnabledChainsProperty, value);
            }
        }

        private static readonly DependencyProperty ControllerProperty = DependencyProperty.RegisterAttached(
            "Controller", typeof(DeclarationEventController), typeof(DeclarationEvent),
            new PropertyMetadata(default(DeclarationEventController)));

        private static readonly Dictionary<string, Func<DM[], IEnumerable<DeclarationChainNode>>>
            ChainNodeConverterDictionary = new Dictionary<string, Func<DM[], IEnumerable<DeclarationChainNode>>>
            {
                {"Down", data => new DeclarationChainNode[] {new DownChainNode(data)}},
                {"Up", data => new DeclarationChainNode[] {new UpChainNode(data)}},
                {"Delay", data => new DeclarationChainNode[] {new DelayChainNode(data)}},
            };

        internal static IEnumerable<DeclarationChainNode> CreateNodes(string key, IEnumerable<DM> metadataArray)
        {
            Func<DM[], IEnumerable<DeclarationChainNode>> func;
            if (ChainNodeConverterDictionary.TryGetValue(key, out func))
            {
                return func(metadataArray as DM[] ?? metadataArray.ToArray());
            }
            return Enumerable.Empty<DeclarationChainNode>();
        }

        public static void RegisterChainNode(string key, Func<DM[], IEnumerable<DeclarationChainNode>> createNode)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (createNode == null) throw new ArgumentNullException(nameof(createNode));
            if (ChainNodeConverterDictionary.ContainsKey(key))
            {
                throw new ArgumentException($"Key \"{key}\" already exists", nameof(key));
            }

            ChainNodeConverterDictionary.Add(key, createNode);
        }
    }
}
