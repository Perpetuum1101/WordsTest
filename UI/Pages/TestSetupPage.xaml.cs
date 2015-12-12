using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using WordTes.UI.Models;

namespace WordTes.UI.Pages
{
    public sealed partial class TestSetupPage
    {
        private readonly TestSetupPageModel _pageModel;
        private bool _dialogShown;

        public TestSetupPage()
        {
            InitializeComponent();

            InformationDialog.Closed += (sender, args) => _dialogShown = false; 

            _pageModel = MainGrid.DataContext as TestSetupPageModel;
            if (_pageModel != null)
            {
                _pageModel.OnPopupEnabled += OnPopupEnabled;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var model = e.Parameter as TestSetupModel;

            if (model != null)
            {
                if (_pageModel != null)
                {
                    if (model.TestName == TestSetupModel.DefaultTestName ||
                        !_pageModel.Tests.Contains(model.TestName))
                    {
                        model.TestName = TestSetupPageModel.DefaulTestName;
                    }

                    _pageModel.CurrentTest = model.TestName;
                    
                    if (_pageModel.TestName == TestSetupPageModel.DefaulTestName)
                    {
                        _pageModel.Items = new ObservableCollection<TestItemWrapper>();
                        foreach (var item in model.Items)
                        {
                            _pageModel.Items.Add(new TestItemWrapper
                            {
                                Item = item,
                            });
                        }
                    }
                }
            }

            base.OnNavigatedTo(e);
        }

        private async void OnPopupEnabled()
        {
            if (_dialogShown)
            {
                return;
            }

            _dialogShown = true;
            await InformationDialog.ShowAsync();
        }
    }
}
