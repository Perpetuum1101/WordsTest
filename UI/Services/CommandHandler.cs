using System;
using System.Windows.Input;
using WordTes.UI.Models;

namespace WordTes.UI.Services
{
    public class CommandHandler : ICommand
    {
        private readonly Action _action;
        private readonly Action<TestItemWrapper> _paramAction;
        private readonly bool _canExecute;

        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public CommandHandler(Action<TestItemWrapper> action, bool canExecute)
        {
            _paramAction = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            if (_paramAction != null)
            {
                _paramAction((TestItemWrapper)parameter);
            }
            else
            {
                _action();
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
