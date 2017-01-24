using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
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
        private Dictionary<int, DeviceInfo> _pressingDevices;
        private DispatcherTimer _timer;

        private void OnInputStarted(object sender, DeviceInputStartedEventArgs e)
        {
            _collectedChains = EnumerateTree(e.OriginalSource, el =>
            {
                var collection = (DeclarationChainCollection) el.GetValue(DeclarationEvent.EnabledChainsProperty);
                return new KeyValuePair<DeclarationChainCollection, UIElement>(collection, el);
            }).Where(x => x.Key != null).SelectMany(pair =>
                pair.Key.Select(chain => new KeyValuePair<DeclarationChain, UIElement>(chain, pair.Value)))
                .ToDictionary(x => x.Key, x => x.Value);

            _collectedManager = new EventStateManager(_collectedChains.Keys) {Confirmed = OnConfirmed};
            _knownManager = new EventStateManager(DeclarationEvent.GetKnownEventChains());
            _pressingDevices = new Dictionary<int, DeviceInfo>();

            _timer?.Stop();
            _timer = new DispatcherTimer(DispatcherPriority.Send)
            {
                Interval = DelayInfo.GetDefaultDelay(e.Type),
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void OnInputCompleted(object sender, DeviceInputCompletedEventArgs e)
        {
            _collectedChains = null;
            _collectedManager = null;
            _knownManager = null;
            _pressingDevices = null;

            _timer.Tick -= Timer_Tick;
            _timer.Stop();
            _timer = null;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Tick -= Timer_Tick;

            
        }

        private void OnDown(object sender, DeviceInputEventArgs e)
        {
            UpdatePressingInfo(e.Id, x => x.DownTime = DateTime.Now);

            var node = new DownChainNode();
            _collectedManager.Step(node);
            _knownManager.Step(node);
        }

        private void OnMove(object sender, DeviceInputEventArgs e)
        {
            var elapsed = DateTime.Now - GetPressingInfo(e.Id, x => x.DownTime);
            var node = elapsed > DelayInfo.GetDefaultDelay(e.Type) ? new MoveChainNode(DE.Long) : new MoveChainNode(DE.Short);

            _collectedManager.Step(node);
            _knownManager.Step(node);
        }

        private void OnUp(object sender, DeviceInputEventArgs e)
        {
            var node = new UpChainNode();
            _collectedManager.Step(node);
            _knownManager.Step(node);
        }

        private void OnHover(object sender, DeviceInputEventArgs e)
        {
        }

        private void OnConfirmed(DeclarationChain chain)
        {
            var source = _collectedChains[chain];

        }

        private T GetPressingInfo<T>(int id, Func<DeviceInfo, T> get)
        {
            DeviceInfo info;
            if (_pressingDevices.TryGetValue(id, out info))
            {
                return get(info);
            }
            return default(T);
        }

        private void UpdatePressingInfo(int id, Action<DeviceInfo> update)
        {
            DeviceInfo info;
            if (!_pressingDevices.TryGetValue(id, out info))
            {
                info = new DeviceInfo();
                _pressingDevices[id] = info;
            }
            update(info);
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

        private class DeviceInfo
        {
            internal DateTime DownTime { get; set; }
        }
    }
}
