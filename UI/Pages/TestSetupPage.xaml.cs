using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WordsTest.Model;
using WordTes.UI.Models;

namespace WordTes.UI.Pages
{
    public sealed partial class TestSetupPage
    {
        public TestSetupPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var items = (ObservableCollection<TestItem>)e.Parameter;

            if (items != null && items.Count != 0)
            {
                var data = MainGrid.DataContext as TestSetupPageModel;

                if (data != null)
                {
                    foreach (var item in items)
                    {
                        data.Items.Add(new TestItemWrapper
                        {
                            Item = item,
                        });
                    }

                    data.Items.First().NotFirst = false;
                    data.Items.Last().Last = true;
                }
            }

            base.OnNavigatedTo(e);
        }

        private void FocusTextBoxOnLoad(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            textbox?.Focus(FocusState.Keyboard);
        }
    }
}
