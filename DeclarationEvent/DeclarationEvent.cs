using System.Windows;

namespace Walterlv.Events
{
    public sealed class DeclarationEvent
    {
        public static readonly DependencyProperty IsHostProperty =
            DependencyProperty.RegisterAttached(
                "IsHost", typeof (bool), typeof (DeclarationEvent),
                new PropertyMetadata(false, OnIsHostChanged));

        public static void SetIsHost(DependencyObject element, bool value)
        {
            element.SetValue(IsHostProperty, value);
        }

        public static bool GetIsHost(DependencyObject element)
        {
            return (bool) element.GetValue(IsHostProperty);
        }

        public static readonly DependencyProperty ControllerProperty = DependencyProperty.RegisterAttached(
            "Controller", typeof (DeclarationEventController), typeof (DeclarationEvent),
            new PropertyMetadata(default(DeclarationEventController)));

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
    }
}
