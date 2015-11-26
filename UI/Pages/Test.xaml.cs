using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WordTes.UI.Models;

namespace WordTes.UI.Pages
{
    public sealed partial class Test
    {
        private readonly TestPageModel _pageModel;

        public Test()
        {
            InitializeComponent();
            _pageModel = MainGrid.DataContext as TestPageModel;
            if (_pageModel != null)
            {
                _pageModel.OnPopupEnabled += OnPopupEnabled;
                _pageModel.OnPopupDisabled += OnPopupDiabled;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var model = (TestSetupModel) e.Parameter;
            if (model == null)
            {
                return;
            }

            _pageModel.Items = model.Items;
            _pageModel.TestName = model.TestName;
            _pageModel.Init(model.CorrectnessRate);

            base.OnNavigatedTo(e);
        }

        private async void OnPopupEnabled()
        {
            await ConfirmationDialog.ShowAsync();
        }

        private void OnPopupDiabled()
        {
            ConfirmationDialog.Hide();
        }

    }
}
