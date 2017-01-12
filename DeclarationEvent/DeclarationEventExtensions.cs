using System.Windows;

namespace Walterlv.Events
{
    public static class DeclarationEventExtensions
    {
        public static void DeclareEvents(this UIElement element, DC chain)
        {
            var collection = DeclarationEvent.GetEnabledChains(element);
            collection.Add(chain.ToChain());
        }

        public static void ClearEventDeclaring(this UIElement element)
        {
            DeclarationEvent.SetEnabledChains(element, null);
        }
    }
}
