using System.Windows;
using Walterlv.Events;
using static Walterlv.Events.DE;

namespace Cvte.DEDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DTest3.DeclareEvents(new DC().Down().Move(Optional, Near, Confirmed).Up());
        }
    }

    public static class XxExtensions
    {
        public static DC Holding(this DC dc)
        {
            return dc;
        }
    }
}
