﻿using Windows.UI.Xaml;

namespace WordTes.UI
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TestSetupPage), null);
        }
    }
}
