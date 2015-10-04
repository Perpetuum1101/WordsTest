using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Media;
using UI.Annotations;
using WordsTest.Model;
using WordTes.UI.Services;
using WordTest.Manager;

namespace WordTes.UI.Models
{
    public class TestPageModel : INotifyPropertyChanged
    {
        private ICommand _checkCommand;
        private readonly bool _canExecute;
        private IManager _manager;
        private Result _result;
        private string _currentWord;
        private string _currentTransaltion;
        private string _checkButtonText;
        private TestState _testState;
        private string _progress;

        public TestPageModel()
        {
            _canExecute = true;
            CurrentTestState = TestState.Check;

            _result = new Result
            {
                Color = new SolidColorBrush(Colors.White),
                Message = "Result goes here"
            };
        }

        public void Init()
        {
            _manager = new Manager(Items, 90);
            CurrentWord = _manager.Get().Word;
            Progress = "Progress " + _manager.Progress;
        }

        public IList<TestItem> Items { get; set; }

        public string CurrentWord
        {
            get { return _currentWord; }
            set
            {
                _currentWord = value;
                OnPropertyChanged(nameof(CurrentWord));
            }
        }

        public string CurrentTranslation
        {
            get { return _currentTransaltion; }
            set
            {
                _currentTransaltion = value;
                OnPropertyChanged(nameof(CurrentTranslation));
            }
        }

        public string CheckButtonText
        {
            get { return _checkButtonText; }
            set
            {
                _checkButtonText = value;
                OnPropertyChanged(nameof(CheckButtonText));
            }
        }

        public string Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        public TestState CurrentTestState
        {
            get { return _testState; }
            set
            {
                _testState = value;
                switch (value)
                {
                    case TestState.Check:
                        CheckButtonText = "Check";
                        break;
                    case TestState.Next:
                        CheckButtonText = "Next";
                        break;
                    case TestState.Done:
                        CheckButtonText = "Done";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        public Result Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public ICommand CheckCommand => _checkCommand ?? (_checkCommand = new CommandHandler(Check, _canExecute));

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Check()
        {
            switch (CurrentTestState)
            {
                case TestState.Check:
                    var result = _manager.Check(CurrentTranslation);
                    ChangeStateByResult(result);
                    break;
                case TestState.Next:
                    CurrentWord = _manager.Get().Word;
                    CurrentTranslation = null;
                    Result.Message = string.Empty;
                    CurrentTestState = TestState.Check;
                    break;
                case TestState.Done:
                    App.NavigationService.Navigate<Pages.TestSetupPage>(Items);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ChangeStateByResult(CheckResult result)
        {
            switch (result)
            {
                case CheckResult.Correct:
                    Result.Message = "Correct!";
                    Progress = "Progress " + _manager.Progress;
                    Result.Color = new SolidColorBrush(Colors.Green);
                    CurrentTestState = TestState.Next;
                    break;
                case CheckResult.Incorrect:
                    Result.Message = "Incorrect!";
                    Result.Color = new SolidColorBrush(Colors.Red);
                    CurrentTestState = TestState.Next;
                    break;
                case CheckResult.Done:
                    Result.Message = "Correct!";
                    Result.Color = new SolidColorBrush(Colors.Green);
                    CurrentTestState = TestState.Done;
                    Progress = "All done!";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
