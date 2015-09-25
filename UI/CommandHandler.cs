using System;
using System.Windows.Input;
using TestModel;

namespace WordTes.UI
{
    public class CommandHandler : ICommand
    {
        private readonly Action _action;
        private readonly Action<TestItem> _paramAction;
        private readonly bool _canExecute;

        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public CommandHandler(Action<TestItem> action, bool canExecute)
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
                _paramAction((TestItem)parameter);
            }
            else
            {
                _action();
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
