using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using UI.Annotations;
using WordsTest.Model;
using WordTes.UI.Services;

namespace WordTes.UI.Models
{
    public class TestItemWrapper : INotifyPropertyChanged
    {
        private TestItem _item;
        private bool _first;
        private bool _last;

        public TestItemWrapper()
        {
            Last = false;
            NotFirst = true;
            Item = new TestItem();
        }

        public TestItem Item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

        public bool Last
        {
            get { return _last; }
            set
            {
                _last = value;
                OnPropertyChanged(nameof(Last));
            }
        }

        public bool NotFirst
        {
            get { return _first; }
            set
            {
                _first = value;
                OnPropertyChanged(nameof(NotFirst));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var visible = (bool) value;

            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var visible = (Visibility) value;

            return visible == Visibility.Visible;
        }
    }

    public class TestSetupPageModel : INotifyPropertyChanged
    {
        private ICommand _addCommand;
        private ICommand _removeCommand;
        private ICommand _startTestCommand;
        private readonly bool _canExecute;

        public TestSetupPageModel()
        {
            Items = new ObservableCollection<TestItemWrapper>
            {
                new TestItemWrapper
                {
                    NotFirst = false,
                    Last = true
                }
            };

            _canExecute = true;
        }

        public ObservableCollection<TestItemWrapper> Items { get; set; }

        public ICommand AddCommand => _addCommand ??
                                      (_addCommand = new CommandHandler(Add, _canExecute));

        public ICommand RemoveCommand => _removeCommand ??
                                         (_removeCommand =
                                             new CommandHandler(Remove, _canExecute));

        public ICommand StartTestCommand => _startTestCommand ??
                                            (_startTestCommand =
                                                new CommandHandler(StartTest, _canExecute));

        public void Add()
        {
            if (Items.Count != 0)
            {
                Items.Last().Last = false;
            }
            Items.Add(new TestItemWrapper {Last = true});
        }

        public void Remove(TestItemWrapper item)
        {
            Items.Remove(item);

            if (Items.Count != 0)
            {
                Items.Last().Last = true;
            }   
        }

        public void StartTest()
        {
            var items = Items.Select(i => i.Item).ToList();
            App.NavigationService.Navigate<Pages.Test>(items);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
