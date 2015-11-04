using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UI.Annotations;
using WordTes.UI.Services;
using WordTest.Repository;

namespace WordTes.UI.Models
{
    public class TestSetupPageModel : INotifyPropertyChanged
    {
        #region Constants

        public string DefaulTestName = "New";

        #endregion

        #region Private

        private ICommand _addCommand;
        private ICommand _removeCommand;
        private ICommand _startTestCommand;
        private ICommand _saveCommand;
        private ICommand _deleteTestCommand;
        private readonly bool _canExecute;
        private ITestRepository _repository;
        private string _currentTest;
        private bool _showTestName;
        private string _testName;
        private bool _saveIsEnabled;
        private bool _showTestDeleteButton;
        private bool _focusTestName;

        #endregion

        #region Constructor

        public TestSetupPageModel()
        {
            Items = new ObservableCollection<TestItemWrapper>();

            AddInitialTestItem();

            Tests = new ObservableCollection<string>();
            RefreshTestList(DefaulTestName);
            _showTestDeleteButton = false;
            _canExecute = true;
        }

        #endregion

        #region Properties

        public bool FocusTestName
        {
            get { return _focusTestName; }
            set
            {
                _focusTestName = value;
                OnPropertyChanged(nameof(FocusTestName));
            }
        }

        public bool ShowTestDeleteButton
        {
            get { return _showTestDeleteButton; }
            set
            {
                _showTestDeleteButton = value;
                OnPropertyChanged(nameof(ShowTestDeleteButton));
            }
        }

        public string CurrentTest
        {
            get { return _currentTest; }

            set
            {
                _currentTest = value;
                OnPropertyChanged(nameof(CurrentTest));
                ShowTestName = _currentTest == DefaulTestName;
                FocusTestName = ShowTestName;
                ShowTestDeleteButton = !ShowTestName;

                TestName = !ShowTestName ? _currentTest : null;

                ChangeTest(_currentTest);
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
                SaveIsEnabled = !string.IsNullOrWhiteSpace(_testName);
                OnPropertyChanged(nameof(TestName));
            }
        }

        #endregion

        #region Fields

        public ITestRepository Repository => _repository ?? (_repository = new TestRepository());

        public ICommand AddCommand =>
            _addCommand ?? (_addCommand = new CommandHandler(Add, _canExecute));

        public ICommand RemoveCommand =>
            _removeCommand ?? (_removeCommand = new CommandHandler(Remove, _canExecute));

        public ICommand StartTestCommand =>
            _startTestCommand ?? (_startTestCommand = new CommandHandler(StartTest, _canExecute));

        public ICommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new CommandHandler(SaveTest, _canExecute));

        public ICommand DeleteTestCommand =>
            _deleteTestCommand ?? (_deleteTestCommand = new CommandHandler(DeleteTest, _canExecute));

        #endregion

        #region Button Actions

        public void Add()
        {
            if (Items.Count != 0)
            {
                var lastItem = Items.Last();
                if (lastItem != null)
                {
                    lastItem.Last = false;
                    lastItem.Focus = false;
                }
            }

            if (FocusTestName)
            {
                FocusTestName = false;
            }

            var wrapper = new TestItemWrapper
            {
                Last = true,
                Focus = !FocusTestName
            };

            Items.Add(wrapper);
        }

        public void Remove(TestItemWrapper item)
        {
            Items.Remove(item);

            if (Items.Count == 0)
            {
                return;
            }

            var lastItem = Items.Last();
            if (lastItem == null)
            {
                return;
            }
            lastItem.Last = true;
            item.Focus = !FocusTestName;
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
            RefreshTestList(TestName);
        }

        public void DeleteTest()
        {
            Repository.DeleteTest(CurrentTest);
            RefreshTestList(DefaulTestName);
        }

        #endregion

        #region Private Methods

        private void RefreshTestList(string testToSelect)
        {
            Tests.Clear();
            if (testToSelect == DefaulTestName)
            {
                SaveIsEnabled = false;
            }

            var tests = new ObservableCollection<string>(Repository.GetTestsList());
            foreach (var test in tests)
            {
                Tests.Add(test);
            }

            Tests.Insert(0, DefaulTestName);
            CurrentTest = testToSelect;
        }

        private void ChangeTest(string testName)
        {
            if (string.IsNullOrWhiteSpace(testName))
            {
                return;
            }
            Items.Clear();
            if (_currentTest == DefaulTestName)
            {
                AddInitialTestItem();
            }
            else
            {
                LoadTest(testName);
            }
        }

        private void AddInitialTestItem()
        {
            var wrapper = new TestItemWrapper
            {
                NotFirst = false,
                Last = true
            };

            Items.Add(wrapper);
        }


        private void LoadTest(string testName)
        {
            var items = Repository.GeTestItems(testName);

            foreach (var item in items)
            {
                var wrapper = new TestItemWrapper
                {
                    Item = item
                };

                Items.Add(wrapper);
            }

            Items.First().NotFirst = false;
            Items.Last().Last = true;
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
