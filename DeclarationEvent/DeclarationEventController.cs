using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Cvte.Windows.Input;

namespace Walterlv.Events
{
    internal sealed class DeclarationEventController
    {
        private UIElement _owner;
        private DeviceInputSource _eventProcessor;

        public void Attach(UIElement element)
        {
            _owner = element;
            if (DesignerProperties.GetIsInDesignMode(element)) return;

            Start();
        }

        public void Detach()
        {
            if (DesignerProperties.GetIsInDesignMode(_owner)) return;

            Stop();

            _owner = null;
        }

        private void Start()
        {
            _eventProcessor = new DeviceInputSource(_owner);
            _eventProcessor.Attach(OnDown, OnMove, OnUp);
            _eventProcessor.InputStarted += OnInputStarted;
            _eventProcessor.InputCompleted += OnInputCompleted;
            _eventProcessor.InputHover += OnHover;
        }

        private void Stop()
        {
            _eventProcessor.InputHover -= OnHover;
            _eventProcessor.Detach();
            _eventProcessor = null;
        }

        private Dictionary<DeclarationChain, UIElement> _collectedChains;
        private EventStateManager _collectedManager;
        private EventStateManager _knownManager;

        private void OnInputStarted(object sender, DeviceInputStartedEventArgs e)
        {
            _collectedChains = EnumerateTree(e.OriginalSource, el =>
                {
                    var collection = (DeclarationChainCollection) el.GetValue(DeclarationEvent.EnabledChainsProperty);
                    return new KeyValuePair<DeclarationChainCollection, UIElement>(collection, el);
                }).Where(x => x.Key != null)
                .SelectMany(pair =>
                    pair.Key.Select(chain => new KeyValuePair<DeclarationChain, UIElement>(chain, pair.Value)))
                .ToDictionary(x => x.Key, x => x.Value);
            _collectedManager = new EventStateManager(_collectedChains.Keys);
            _knownManager = new EventStateManager(DeclarationEvent.GetKnownEventChains());
        }

        private void OnInputCompleted(object sender, DeviceInputCompletedEventArgs e)
        {
            _collectedChains = null;
        }

        private void OnDown(object sender, DeviceInputEventArgs e)
        {
            
        }

        private void OnMove(object sender, DeviceInputEventArgs e)
        {
        }

        private void OnUp(object sender, DeviceInputEventArgs e)
        {
        }

        private void OnHover(object sender, DeviceInputEventArgs e)
        {
        }

        private static IEnumerable<T> EnumerateTree<T>(UIElement source, Func<UIElement, T> action)
        {
            DependencyObject d = source;
            yield return action(source);
            while (d != null)
            {
                d = VisualTreeHelper.GetParent(d);
                var element = d as UIElement;
                if (element != null)
                {
                    yield return action(element);
                }
            }
        }
    }
}
