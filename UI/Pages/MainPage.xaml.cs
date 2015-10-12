using Windows.UI.Xaml;
using WordTest.Repository;

namespace WordTes.UI.Pages
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            //var repo = new TestRepository();
            //repo.DeleteTest("Test");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TestSetupPage), null);
        }
    }
}
