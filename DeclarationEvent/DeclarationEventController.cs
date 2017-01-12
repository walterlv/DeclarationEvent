using System.ComponentModel;
using System.Windows;
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


        private void OnInputStarted(object sender, DeviceInputStartedEventArgs e)
        {
            
        }

        private void OnInputCompleted(object sender, DeviceInputCompletedEventArgs e)
        {

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
    }
}
