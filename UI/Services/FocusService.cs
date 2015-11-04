using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WordTes.UI.Services
{
    public static class FocusService
    {
        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached(
                "IsFocused",
                typeof (bool),
                typeof (FocusService),
                new PropertyMetadata(false, OnIsFocusedPropertyChanged));

        private static void OnIsFocusedPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs args)
        {
            if (!(bool) args.NewValue)
            {
                return;
            }

            var control = dependencyObject as Control;
            if (control != null)
            {
                control.Loaded += OnControlLoaded;
            }
        }

        private static void OnControlLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var control = sender as Control;
            if (control == null)
            {
                return;
            }

            control.Loaded -= OnControlLoaded;
            control.Focus(FocusState.Keyboard);
        }
    }
}
