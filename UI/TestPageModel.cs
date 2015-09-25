using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Media;
using TestModel;
using UI;
using UI.Annotations;
using WordTest.Manager;

namespace WordTes.UI
{
    public class Result : INotifyPropertyChanged
    {
        private Brush _color;
        private string _message;

        public Brush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum TestState
    {
        Check,
        Next,
        Done
    }

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
        }

        public ObservableCollection<TestItem> Items { get; set; }

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
            if (CurrentTestState == TestState.Check)
            {
                var result = _manager.Check(CurrentTranslation);

                switch (result)
                {
                    case CheckResult.Correct:
                        Result.Message = "Correct!";
                        Result.Color = new SolidColorBrush(Colors.Green);
                        CurrentTestState = TestState.Next;
                        break;
                    case CheckResult.Incorrect:
                        Result.Message = "Incorrect!";
                        Result.Color =new SolidColorBrush(Colors.Red);
                        CurrentTestState = TestState.Next;
                        break;
                    case CheckResult.Done:
                        Result.Message = "Correct!";
                        Result.Color = new SolidColorBrush(Colors.Green);
                        CurrentTestState = TestState.Done;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (CurrentTestState == TestState.Next)
            {
                CurrentWord = _manager.Get().Word;
                CurrentTranslation = null;
                Result.Message = string.Empty;
                CurrentTestState = TestState.Check;
            }
            else if (CurrentTestState == TestState.Done)
            {
                App.NavigationService.Navigate<TestSetupPage>();
            }
        }
    }
}
