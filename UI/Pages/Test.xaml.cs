using Windows.UI.Xaml.Navigation;
using WordTes.UI.Models;

namespace WordTes.UI.Pages
{
    public sealed partial class Test
    {
        public Test()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var model = (TestSetupModel)e.Parameter;
            if (model == null)
            {
                return;
            }

            var data = MainGrid.DataContext as TestPageModel;

            if (data != null)
            {
                data.Items = model.Items;
                data.TestName = model.TestName;
                data.Init(model.CorrectnessRate);
            }
            
            base.OnNavigatedTo(e);
        }
    }
}
