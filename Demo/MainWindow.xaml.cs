using System.Windows;
using Walterlv.Events;

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
            DTest3.DeclareEvents(new DC().Down().Up());
        }
    }
}
