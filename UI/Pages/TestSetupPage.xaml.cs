using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WordTes.UI.Pages
{
    public sealed partial class TestSetupPage : Page
    {
        public TestSetupPage()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (Pages.Test), null);
        }
    }
}
