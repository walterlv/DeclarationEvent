using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Walterlv.Events
{
    public sealed class DeclarationEvent
    {
        static DeclarationEvent()
        {
            Register(() => new HoldingEvent());
            Register(() => new TappedEvent());
            Register(() => new DragEvent());
        }

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
            if (true.Equals(e.OldValue))
            {
                var controller = (DeclarationEventController) element.GetValue(ControllerProperty);
                controller?.Detach();
            }
            if (true.Equals(e.NewValue))
            {
                var controller = new DeclarationEventController();
                controller.Attach(element);
            }
        }

        public static readonly DependencyProperty EnabledChainsProperty =
            DependencyProperty.RegisterAttached(
                "EnabledChains", typeof (DeclarationChainCollection), typeof (DeclarationEvent),
                new PropertyMetadata(default(DeclarationChainCollection)));

        public static DeclarationChainCollection GetEnabledChains(DependencyObject element)
        {
            var chains = (DeclarationChainCollection) element.GetValue(EnabledChainsProperty) ??
                         new DeclarationChainCollection();
            return chains;
        }

        public static void SetEnabledChains(DependencyObject element, DeclarationChainCollection value)
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

        private static readonly Dictionary<string, CreateNodeCallback>
            ConverterDictionary = new Dictionary<string, CreateNodeCallback>
            {
                {"Down", data => new DeclarationChainNode[] {new DownChainNode(data)}},
                {"Up", data => new DeclarationChainNode[] {new UpChainNode(data)}},
                {"Move", data => new DeclarationChainNode[] {new MoveChainNode(data)}},
                {"Delay", data => new DeclarationChainNode[] {new DelayChainNode(data)}},
            };

        private static readonly Dictionary<Type, DeclarationChain>
            KnownEventDictionary = new Dictionary<Type, DeclarationChain>();

        internal static IEnumerable<DeclarationChainNode> ConvertNodes(string key, IEnumerable<DE> infos)
        {
            CreateNodeCallback func;
            if (ConverterDictionary.TryGetValue(key, out func))
            {
                return func(infos as DE[] ?? infos.ToArray());
            }
            return Enumerable.Empty<DeclarationChainNode>();
        }

        internal static DeclarationChain[] GetKnownEventChains()
        {
            return KnownEventDictionary.Values.ToArray();
        }

        public static void Register(Func<DeclarationEventBase> createDeclarationEvent)
        {
            if (createDeclarationEvent == null) throw new ArgumentNullException(nameof(createDeclarationEvent));

            var declarationEvent = createDeclarationEvent.Invoke();
            if (declarationEvent == null)
            {
                throw new InvalidOperationException("Delegate cannot return null.");
            }
            
            var name = declarationEvent.GetName();
            var type = declarationEvent.GetType();
            var chain = declarationEvent.GetChain();

            RegisterConverter(name, type, infos => chain);
            RegisterKnownEvent(name, type, chain);
        }

        internal static void RegisterConverter(string name, Type ownerType, CreateNodeCallback createNode)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (ownerType == null) throw new ArgumentNullException(nameof(ownerType));
            if (createNode == null) throw new ArgumentNullException(nameof(createNode));

            if (ConverterDictionary.ContainsKey(name))
            {
                throw new ArgumentException(
                    $"Key \"{name}\" already exists in DeclarationEvent Converters.",
                    nameof(name));
            }

            ConverterDictionary.Add(name, createNode);
        }

        internal static void RegisterKnownEvent(string name, Type ownerType, DeclarationChain chain)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (ownerType == null) throw new ArgumentNullException(nameof(ownerType));
            if (chain == null) throw new ArgumentNullException(nameof(chain));

            if (KnownEventDictionary.ContainsKey(ownerType))
            {
                throw new ArgumentException(
                    $"Type \"{ownerType}\" already exists in Declaration Known Events.",
                    nameof(name));
            }

            KnownEventDictionary.Add(ownerType, chain);
        }
    }

    public delegate IEnumerable<DeclarationChainNode> CreateNodeCallback(DE[] infos);
}
