using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
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
            var model = (TestSetupModel)e.Parameter;

            if (model != null)
            {
                var data = MainGrid.DataContext as TestSetupPageModel;

                if (data != null)
                {
                    if (model.TestName == TestSetupModel.DefaultTestName ||
                        !data.Tests.Contains(model.TestName))
                    {
                        model.TestName = TestSetupPageModel.DefaulTestName;
                    }

                    data.CurrentTest = model.TestName;
                    
                    if (data.TestName == TestSetupPageModel.DefaulTestName)
                    {
                        data.Items = new ObservableCollection<TestItemWrapper>();
                        foreach (var item in model.Items)
                        {
                            data.Items.Add(new TestItemWrapper
                            {
                                Item = item,
                            });
                        }
                    }
                }
            }

            base.OnNavigatedTo(e);
        }
    }
}
