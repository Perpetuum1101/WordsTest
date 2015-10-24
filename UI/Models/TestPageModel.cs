using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Controls;
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
        private ICommand _correctAnswerCommand;
        private readonly bool _canExecute;
        private IManager _manager;
        private Result _result;
        private string _currentWord;
        private string _currentTransaltion;
        private Symbol _checkButtonSymbol;
        private TestState _testState;
        private string _progress;
        private string _correctAnswer;
        private bool _showCorrectAnswerButton;
        private bool _showCorrectAnswer;

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

        public bool ShowCorrectAnswer
        {
            get { return _showCorrectAnswer; }

            set
            {
                _showCorrectAnswer = value; 
                OnPropertyChanged(nameof(ShowCorrectAnswer));
            }

        }

        public string CorrectAnswer
        {
            get { return _correctAnswer; }
            set
            {
                _correctAnswer = value;
                OnPropertyChanged(nameof(CorrectAnswer));
            }
        }

        public bool ShowCorrectAnswerButton
        {
            get { return _showCorrectAnswerButton; }
            set
            {
                _showCorrectAnswerButton = value;
                OnPropertyChanged(nameof(ShowCorrectAnswerButton));
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

        public Symbol CheckButtonSymbol
        {
            get { return _checkButtonSymbol; }
            set
            {
                _checkButtonSymbol = value;
                OnPropertyChanged(nameof(CheckButtonSymbol));
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
                        CheckButtonSymbol = Symbol.Accept;
                        break;
                    case TestState.Next:
                        CheckButtonSymbol = Symbol.Forward;
                        break;
                    case TestState.Done:
                        CheckButtonSymbol = Symbol.Home;
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

        public ICommand ShowAnswerCommand => _correctAnswerCommand ?? (_correctAnswerCommand = new CommandHandler(ShowCorrectAnswerClick, _canExecute));

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
                    ShowCorrectAnswerButton = false;
                    ShowCorrectAnswer = false;
                    break;
                case TestState.Done:
                    App.NavigationService.Navigate<Pages.TestSetupPage>(Items);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ShowCorrectAnswerClick()
        {
            ShowCorrectAnswer = true;
        }

        private void ChangeStateByResult(CheckResult result)
        {
            switch (result.State)
            {
                case CheckState.Correct:
                    Result.Message = string.Format("Correct! ({0}%)", result.Correctness);
                    Progress = "Progress " + _manager.Progress;
                    Result.Color = new SolidColorBrush(Colors.Green);
                    CurrentTestState = TestState.Next;
                    break;
                case CheckState.Incorrect:
                    Result.Message = string.Format("Incorrect! ({0}%)", result.Correctness);
                    Result.Color = new SolidColorBrush(Colors.Red);
                    CurrentTestState = TestState.Next;
                    CorrectAnswer = result.CorrectAnswer;
                    ShowCorrectAnswerButton = true;
                    break;
                case CheckState.Done:
                    Result.Message = string.Format("Correct! ({0}%)", result.Correctness);
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
