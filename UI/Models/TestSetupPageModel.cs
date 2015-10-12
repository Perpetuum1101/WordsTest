using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UI.Annotations;
using WordsTest.Model;
using WordTes.UI.Services;
using WordTest.Repository;

namespace WordTes.UI.Models
{
    public class TestSetupPageModel : INotifyPropertyChanged
    {
        private ICommand _addCommand;
        private ICommand _removeCommand;
        private ICommand _startTestCommand;
        private ICommand _saveCommand;
        private readonly bool _canExecute;
        private ITestRepository _repository;
        private string _currentTest;
        private bool _showTestName;
        private string _testName;
        private bool _saveIsEnabled;

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

            Tests = new ObservableCollection<string>(Repository.GetTestsList());
            Tests.Insert(0, "New");

            _canExecute = true;
        }

        public string CurrentTest
        {
            get { return _currentTest; }

            set
            {
                _currentTest = value;
                OnPropertyChanged(nameof(CurrentTest));
                ShowTestName = _currentTest == "New";
                if (!ShowTestName)
                {
                    TestName = _currentTest;
                    LoadTest(_currentTest);
                }
                else
                {
                    TestName = null;
                }
                

                if (!string.IsNullOrWhiteSpace(_currentTest))
                {
                    LoadTest(_currentTest);
                }
            }
        }

        public ObservableCollection<TestItemWrapper> Items { get; set; }

        public ObservableCollection<string> Tests { get; set; }

        public bool ShowTestName
        {
            get { return _showTestName; }
            set
            {
                _showTestName = value;
                OnPropertyChanged(nameof(ShowTestName));
            }
        }

        public bool SaveIsEnabled
        {
            get { return _saveIsEnabled; }
            set
            {
                _saveIsEnabled = value;
                OnPropertyChanged(nameof(SaveIsEnabled));
            }
        }

        public string TestName
        {
            get { return _testName; }
            set
            {
                _testName = value;
                SaveIsEnabled = !string.IsNullOrWhiteSpace(TestName);
                OnPropertyChanged(nameof(TestName));
            }
        }

        public ITestRepository Repository => _repository ?? (_repository = new TestRepository());

        public ICommand AddCommand =>
            _addCommand ?? (_addCommand = new CommandHandler(Add, _canExecute));

        public ICommand RemoveCommand =>
            _removeCommand ?? (_removeCommand = new CommandHandler(Remove, _canExecute));

        public ICommand StartTestCommand =>
            _startTestCommand ?? (_startTestCommand = new CommandHandler(StartTest, _canExecute));

        public ICommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new CommandHandler(SaveTest, _canExecute));



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

        public void SaveTest()
        {
            var testItems = Items.Select(item => item.Item).ToList();

            Repository.SaveTest(TestName, testItems);
            
        }

        public void LoadTest(string testName)
        {
            Items.Clear();
            if (_currentTest == "New")
            {
                Items.Add(new TestItemWrapper
                {
                    NotFirst = false,
                    Last = true
                });

            }
            else
            {
                var items = Repository.GeTestItems(testName);

                foreach (var item in items)
                {
                    Items.Add(new TestItemWrapper
                    {
                        Item = item
                    });
                }

                Items.First().NotFirst = false;
                Items.Last().Last = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
