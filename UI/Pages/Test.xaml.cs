using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using WordsTest.Model;
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
            var items = (IList<TestItem>)e.Parameter;

            var data = MainGrid.DataContext as TestPageModel;

            if (data != null)
            {
                data.Items = items;
                data.Init();
            }
            
            base.OnNavigatedTo(e);
        }
    }
}
