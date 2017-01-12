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
    }
}
