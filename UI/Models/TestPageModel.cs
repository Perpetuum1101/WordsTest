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
        #region Private

        private ICommand _checkCommand;
        private ICommand _correctAnswerCommand;
        private ICommand _backCommand;
        private ICommand _popupButtonOkCommand;
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
        private bool _popupEnabled;

        #endregion

        #region Constructor

        public TestPageModel()
        {
            _canExecute = true;
            CurrentTestState = TestState.Check;

            _result = new Result
            {
                Color = new SolidColorBrush(Colors.White)
            };
        }

        #endregion

        #region Properties

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

        public string TestName { get; set; }

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

        public bool PopupEnabled
        {
            get { return _popupEnabled; }

            set
            {
                _popupEnabled = value;
                OnPropertyChanged(nameof(PopupEnabled));
            }
        }

        #endregion

        #region Fields


        public ICommand CheckCommand => _checkCommand ??
                                       (_checkCommand = new CommandHandler(Check, _canExecute));

        public ICommand ShowAnswerCommand => _correctAnswerCommand ??
                                            (_correctAnswerCommand =
                                                 new CommandHandler(
                                                     ShowCorrectAnswerClick,
                                                     _canExecute));

        public ICommand BackCommand => _backCommand ??
                                      (_backCommand = new CommandHandler(Back, _canExecute));

        public ICommand PopupButtonOkCommand => _popupButtonOkCommand ??
                                                (_popupButtonOkCommand =
                                                    new CommandHandler(
                                                        BackPopupOkClick,
                                                        _canExecute));

        #endregion

        #region Public Methos

        public void Init(int correctnessRate)
        {
            _manager = new Manager(Items, correctnessRate);
            CurrentWord = _manager.Get().Word;
            Progress = "Progress " + _manager.Progress;
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
                    Back();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ShowCorrectAnswerClick()
        {
            ShowCorrectAnswer = true;
        }

        public void Back()
        {
            if (_manager.Completed == _manager.OriginalLength)
            {
                GoBack();
            }
            else
            {
                OnPopupEnabledInvoke();
            }
        }

        public void BackPopupOkClick()
        {
            OnPopupDisabledInvoke();
            GoBack();
        } 

        #endregion

        #region Private Methods

        private void ChangeStateByResult(CheckResult result)
        {
            switch (result.State)
            {
                case CheckState.Correct:
                    ChangeState(result, TestState.Next, "Progress " + _manager.Progress);
                    break;
                case CheckState.Incorrect:
                    Result.Message = string.Format("Incorrect! ({0}%)", result.Correctness);
                    Result.Color = new SolidColorBrush(Colors.Red);
                    CurrentTestState = TestState.Next;
                    CorrectAnswer = result.CorrectAnswer;
                    ShowCorrectAnswerButton = true;
                    break;
                case CheckState.Done:
                    ChangeState(result, TestState.Done, "All done!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ChangeState(CheckResult result, TestState state, string progressText)
        {
            Result.Message = string.Format("Correct! ({0}%)", result.Correctness);
            Result.Color = new SolidColorBrush(Colors.Green);
            CurrentTestState = state;

            if (result.Correctness != 100)
            {
                CorrectAnswer = result.CorrectAnswer;
                ShowCorrectAnswerButton = true;
            }
            Progress = progressText;
        }

        private void GoBack()
        {
            App.NavigationService.Navigate<Pages.TestSetupPage>(new TestSetupModel
            {
                Items = Items,
                TestName = TestName,
            });
        }

        #endregion

        #region Events

        public delegate void  PopupHandler();

        public event PopupHandler OnPopupEnabled;
        public event PopupHandler OnPopupDisabled;

        private void OnPopupEnabledInvoke()
        {
            OnPopupEnabled?.Invoke();
        }

        private void OnPopupDisabledInvoke()
        {
            OnPopupDisabled?.Invoke();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
